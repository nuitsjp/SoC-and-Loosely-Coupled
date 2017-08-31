using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HatPepper.Integrations.Tests
{
    public class GourmetServiceFixture
    {
        /// <summary>
        /// 正常系
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SearchShopsAsync()
        {
            var gourmetService = new GourmetService();
            var actual =
                await gourmetService.SearchShopsAsync(
                    Secrets.HotPepperApiKey,
                    new Location { Latitude = 34.67, Longitude = 135.52 });

            Assert.NotNull(actual);
            Assert.True(actual.Any());
            Assert.NotNull(actual.First().Name);
            Assert.NotNull(actual.First().Genre);
            Assert.NotNull(actual.First().Genre);
        }
    }
}
