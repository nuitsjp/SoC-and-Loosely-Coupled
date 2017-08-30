using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HotPepper.Console.Integrations;
using HotPepper.Console.Usecases;
using Moq;
using Nuits.System.Device.Location;
using Xunit;

namespace HotPepper.Console.Tests.Usecases
{
    public class FindRestaurantsFixture
    {
        [Fact]
        public async Task FindNearbyRestaurantsAsync()
        {
            var geoCoordinateService = new Mock<IGeoCoordinator>();
            var gourmetService = new Mock<IGourmetService>();
            var findRestaurants = new FindRestaurants(geoCoordinateService.Object, gourmetService.Object);

            var timeout = TimeSpan.MaxValue;
            var position = new Location { Latitude = double.MaxValue, Longitude = double.MinValue };
            geoCoordinateService
                .Setup(m => m.GetCurrent(timeout))
                .Returns(() => position);

            var apiKey = "apiKey";
            var shops = new List<Shop>
            {
                new Shop{ Name = "Name0", Genre = "Genre0"},
                new Shop{ Name = "Name1", Genre = "Genre1"}
            };
            
            gourmetService
                .Setup(m => m.SearchShopsAsync(apiKey, position.Latitude, position.Longitude))
                .ReturnsAsync(() => shops);

            var findRestaurantsResult = await findRestaurants.FindNearbyRestaurantsAsync(apiKey, timeout);

            geoCoordinateService.Verify(m => m.GetCurrent(timeout), Times.Once);
            gourmetService.Verify(m => m.SearchShopsAsync(apiKey, position.Latitude, position.Longitude), Times.Once);

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

        [Fact]
        public async Task WhenTimeoutInGeoCoordinateService()
        {
            var geoCoordinateService = new Mock<IGeoCoordinator>();
            var gourmetService = new Mock<IGourmetService>();
            var findRestaurants = new FindRestaurants(geoCoordinateService.Object, gourmetService.Object);

            var timeout = TimeSpan.MaxValue;
            geoCoordinateService
                .Setup(m => m.GetCurrent(timeout))
                .Returns(() => null);

            var apiKey = "apiKey";

            var findRestaurantsResult = await findRestaurants.FindNearbyRestaurantsAsync(apiKey, timeout);

            geoCoordinateService.Verify(m => m.GetCurrent(timeout), Times.Once);
            gourmetService.Verify(m => m.SearchShopsAsync(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()), Times.Never);

            Assert.NotNull(findRestaurantsResult);
            Assert.Equal(FindRestaurantsResultStatus.Timeout, findRestaurantsResult.Status);
            Assert.Null(findRestaurantsResult.Restaurants);
        }

        [Fact]
        public async Task WhenHttpRequestExceptionInGourmetService()
        {
            var geoCoordinateService = new Mock<IGeoCoordinator>();
            var gourmetService = new Mock<IGourmetService>();
            var findRestaurants = new FindRestaurants(geoCoordinateService.Object, gourmetService.Object);

            var timeout = TimeSpan.MaxValue;
            var position = new Location { Latitude = double.MaxValue, Longitude = double.MinValue };
            geoCoordinateService
                .Setup(m => m.GetCurrent(timeout))
                .Returns(() => position);

            var apiKey = "apiKey";
            gourmetService
                .Setup(m => m.SearchShopsAsync(apiKey, position.Latitude, position.Longitude))
                .ThrowsAsync(new HttpRequestException());

            var findRestaurantsResult = await findRestaurants.FindNearbyRestaurantsAsync(apiKey, timeout);

            geoCoordinateService.Verify(m => m.GetCurrent(timeout), Times.Once);
            gourmetService.Verify(m => m.SearchShopsAsync(apiKey, position.Latitude, position.Longitude), Times.Once);

            Assert.NotNull(findRestaurantsResult);
            Assert.Equal(FindRestaurantsResultStatus.NetworkError, findRestaurantsResult.Status);
            Assert.Null(findRestaurantsResult.Restaurants);
        }
    }
}
