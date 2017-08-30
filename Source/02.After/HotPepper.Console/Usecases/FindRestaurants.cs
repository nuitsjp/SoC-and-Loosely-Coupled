using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HotPepper.Console.Integrations;
using Nuits.System.Device.Location;

namespace HotPepper.Console.Usecases
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
            var restaurants = new List<Restaurant>();

            var position = _geoCoordinator.GetCurrent(timeout);
            if (position != null)
            {
                try
                {
                    var shops = await _gourmetService.SearchShopsAsync(apiKey, position.Latitude, position.Longitude);
                    foreach (var shop in shops)
                    {
                        restaurants.Add(
                            new Restaurant
                            {
                                Name = shop.Name,
                                Genre = shop.Genre
                            });
                    }

                    var findRestaurantsResult = new FindRestaurantsResult
                    {
                        Status = FindRestaurantsResultStatus.Ok,
                        Restaurants = restaurants
                    };
                    return findRestaurantsResult;
                }
                catch (HttpRequestException)
                {
                    return new FindRestaurantsResult { Status = FindRestaurantsResultStatus.NetworkError };
                }
            }
            else
            {
                return new FindRestaurantsResult { Status = FindRestaurantsResultStatus.Timeout };
            }
        }
    }
}
