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
        public async Task<GourmetSearchResult> FindNearbyRestaurantsAsync(string apiKey, TimeSpan timeout)
        {
            GeoCoordinator geoCoordinator = new GeoCoordinator();
            var location = geoCoordinator.GetCurrent(timeout);

            GourmetService gourmetService = new GourmetService();
            return await gourmetService.SearchGourmetInfosAsync(apiKey, location);
        }
    }
}
