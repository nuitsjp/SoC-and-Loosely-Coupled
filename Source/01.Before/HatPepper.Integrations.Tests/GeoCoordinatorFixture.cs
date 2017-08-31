using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HatPepper.Integrations.Tests
{
    public class GeoCoordinatorFixture
    {
        /// <summary>
        /// 正常系
        /// </summary>
        [Fact]
        public void GetCurrent()
        {
            var geoCoordinator = new GeoCoordinator();

            var actual = geoCoordinator.GetCurrent(TimeSpan.FromMinutes(1));

            Assert.NotNull(actual);
            // 実際の位置情報が取得されているか正しくテストするのは困難なため
            // 今回は値が返ってくれば良いものとする
            // 正しい値が返ってくるかは結合（手動）テストにて確認する
            Assert.False(double.IsNaN(actual.Latitude));
            Assert.False(double.IsNaN(actual.Longitude));
        }

        /// <summary>
        /// 時間内に現在地が取得できなかった場合
        /// </summary>
        [Fact]
        public void WhenTimeoutInGetCurrent()
        {
            var geoCoordinator = new GeoCoordinator();

            var actual = geoCoordinator.GetCurrent(TimeSpan.Zero);

            Assert.Null(actual);
        }
    }
}
