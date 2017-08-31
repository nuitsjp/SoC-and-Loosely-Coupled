using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HatPepper.Integrations;
using Moq;
using Xunit;

namespace HatPepper.Usecases.Tests
{
    public class FindRestaurantsFixture
    {
        /// <summary>
        /// 正常系
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FindNearbyRestaurantsAsync()
        {
            // Mockを作成した上で、テスト対象のインスタンスを生成する
            var geoCoordinateService = new Mock<IGeoCoordinator>();
            var gourmetService = new Mock<IGourmetService>();
            var findRestaurants = new FindRestaurants(geoCoordinateService.Object, gourmetService.Object);

            // 現在地情報取得時のMockの振る舞いをセットアップする
            var timeout = TimeSpan.MaxValue;
            var location = new Location { Latitude = double.MaxValue, Longitude = double.MinValue };
            geoCoordinateService
                .Setup(m => m.GetCurrent(timeout))
                .Returns(() => location);

            // グルメサーチAPIが呼び出された際の振る舞いをセットアップする
            var apiKey = "apiKey";
            var shops = new List<GourmetInfo>
            {
                new GourmetInfo{ ShopName = "Name0", Genre = "Genre0"},
                new GourmetInfo{ ShopName = "Name1", Genre = "Genre1"}
            };
            gourmetService
                .Setup(m => m.SearchGourmetInfosAsync(apiKey, location))
                .ReturnsAsync(() => shops);

            // 現在地周辺のレストラン一覧を取得する
            var findRestaurantsResult = await findRestaurants.FindNearbyRestaurantsAsync(apiKey, timeout);

            // FindNearbyRestaurantsAsyncメソッドの呼び出し時に渡されたtimeoutを引数に
            // 一度だけGetCurrentが呼び出されたことを確認する
            geoCoordinateService.Verify(m => m.GetCurrent(timeout), Times.Once);

            // FindNearbyRestaurantsAsyncメソッドで渡されたpiKeyと
            // GetCurrentの呼び出し結果で取得されたLocationを引数に
            // SearchShopsAsyncが一度だけ呼び出されたことを確認する
            gourmetService.Verify(m => m.SearchGourmetInfosAsync(apiKey, location), Times.Once);

            // 取得結果を確認する
            Assert.NotNull(findRestaurantsResult);
            Assert.Equal(FindRestaurantsResultStatus.Ok, findRestaurantsResult.Status);

            var restaurants = findRestaurantsResult.Restaurants;
            Assert.NotNull(restaurants);
            Assert.Equal(2, restaurants.Count);

            Assert.NotNull(restaurants[0]);
            Assert.Equal("Name0", restaurants[0].Name);
            Assert.Equal("Genre0", restaurants[0].Genre);

            Assert.NotNull(restaurants[1]);
            Assert.Equal("Name1", restaurants[1].Name);
            Assert.Equal("Genre1", restaurants[1].Genre);
        }

        /// <summary>
        /// 現在地取得時に、タイムアウトが派生した場合
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task WhenTimeoutInGeoCoordinateService()
        {
            var geoCoordinateService = new Mock<IGeoCoordinator>();
            var gourmetService = new Mock<IGourmetService>();
            var findRestaurants = new FindRestaurants(geoCoordinateService.Object, gourmetService.Object);

            // タイムアウトを発生た状態を表現するためGetCurrentの戻り値にnullを返す
            var timeout = TimeSpan.MaxValue;
            geoCoordinateService
                .Setup(m => m.GetCurrent(timeout))
                .Returns(() => null);

            var apiKey = "apiKey";

            var findRestaurantsResult = await findRestaurants.FindNearbyRestaurantsAsync(apiKey, timeout);

            geoCoordinateService.Verify(m => m.GetCurrent(timeout), Times.Once);
            gourmetService.Verify(m => m.SearchGourmetInfosAsync(It.IsAny<string>(), It.IsAny<Location>()), Times.Never);

            Assert.NotNull(findRestaurantsResult);
            Assert.Equal(FindRestaurantsResultStatus.Timeout, findRestaurantsResult.Status);
            Assert.Null(findRestaurantsResult.Restaurants);
        }

        /// <summary>
        /// グルメサーチAPI呼び出し時にHttpRequestExceptionが発生した場合
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task WhenHttpRequestExceptionInGourmetService()
        {
            var geoCoordinateService = new Mock<IGeoCoordinator>();
            var gourmetService = new Mock<IGourmetService>();
            var findRestaurants = new FindRestaurants(geoCoordinateService.Object, gourmetService.Object);

            var timeout = TimeSpan.MaxValue;
            var location = new Location { Latitude = double.MaxValue, Longitude = double.MinValue };
            geoCoordinateService
                .Setup(m => m.GetCurrent(timeout))
                .Returns(() => location);

            // SearchShopsAsyncが呼び出された場合にHttpRequestExceptionをスローするようにセットアップする
            var apiKey = "apiKey";
            gourmetService
                .Setup(m => m.SearchGourmetInfosAsync(apiKey, location))
                .ThrowsAsync(new HttpRequestException());

            var findRestaurantsResult = await findRestaurants.FindNearbyRestaurantsAsync(apiKey, timeout);

            geoCoordinateService.Verify(m => m.GetCurrent(timeout), Times.Once);
            gourmetService.Verify(m => m.SearchGourmetInfosAsync(apiKey, location), Times.Once);

            Assert.NotNull(findRestaurantsResult);
            Assert.Equal(FindRestaurantsResultStatus.NetworkError, findRestaurantsResult.Status);
            Assert.Null(findRestaurantsResult.Restaurants);
        }
    }
}
