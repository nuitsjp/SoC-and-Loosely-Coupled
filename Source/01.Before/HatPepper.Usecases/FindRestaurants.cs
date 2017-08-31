using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HatPepper.Integrations;

namespace HatPepper.Usecases
{
    public class FindRestaurants
    {
        public async Task<IEnumerable<Shop>> FindNearbyRestaurantsAsync(string apiKey, TimeSpan timeout)
        {
            GeoCoordinator geoCoordinator = new GeoCoordinator();
            var location = geoCoordinator.GetCurrent(timeout);

            GourmetService gourmetService = new GourmetService();
            var result = await gourmetService.SearchGourmetInfosAsync(apiKey, location);
            return result.Results.Shops;
        }
    }
}
