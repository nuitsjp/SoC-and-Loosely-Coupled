using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HatPepper.Integrations;

namespace HatPepper.Usecases
{
    public class FindRestaurants : IFindRestaurants
    {
        private readonly IGeoCoordinator _geoCoordinator;
        private readonly IGourmetService _gourmetService;

        public FindRestaurants(IGeoCoordinator geoCoordinator, IGourmetService gourmetService)
        {
            _geoCoordinator = geoCoordinator;
            _gourmetService = gourmetService;
        }

        public async Task<FindRestaurantsResult> FindNearbyRestaurantsAsync(string apiKey, TimeSpan timeout)
        {
            var location = _geoCoordinator.GetCurrent(timeout);
            if (location == null)
                return new FindRestaurantsResult { Status = FindRestaurantsResultStatus.Timeout };

            try
            {
                var shops = await _gourmetService.SearchShopsAsync(apiKey, location);
                var findRestaurantsResult = new FindRestaurantsResult
                {
                    Status = FindRestaurantsResultStatus.Ok,
                    Restaurants = shops.Select(shop => new Restaurant
                    {
                        Name = shop.Name,
                        Genre = shop.Genre
                    }).ToList()
                };
                return findRestaurantsResult;
            }
            catch (HttpRequestException)
            {
                return new FindRestaurantsResult { Status = FindRestaurantsResultStatus.NetworkError };
            }
        }
    }
}
