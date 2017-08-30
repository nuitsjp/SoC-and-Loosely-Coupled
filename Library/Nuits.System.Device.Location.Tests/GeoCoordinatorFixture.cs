using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nuits.System.Device.Location.Tests
{
    public class GeoCoordinatorFixture
    {
        [Fact]
        public void GetCurrent()
        {
            var coordinator = new GeoCoordinator();
            var actual = coordinator.GetCurrent(TimeSpan.FromMinutes(1));

            Assert.NotNull(actual);
            Assert.NotEqual(double.NaN, actual.Latitude);
            Assert.NotEqual(double.NaN, actual.Longitude);
        }

        [Fact]
        public void TimeoutInGetCurrent()
        {
            var coordinator = new GeoCoordinator();
            var actual = coordinator.GetCurrent(TimeSpan.FromMinutes(0));

            Assert.Null(actual);
        }
    }
}
