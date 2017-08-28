using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HotPepper.Console.Integrations;
using HotPepper.Console.Integrations.GeoCoordinate;
using HotPepper.Console.Integrations.Gourmet;

namespace HotPepper.Console.Usecases
{
    public class FindRestaurants : IFindRestaurants
    {
        private readonly IGeoCoordinateService _geoCoordinateService;
        private readonly IGourmetService _gourmetService;

        public FindRestaurants(IGeoCoordinateService geoCoordinateService, IGourmetService gourmetService)
        {
            _geoCoordinateService = geoCoordinateService;
            _gourmetService = gourmetService;
        }

        public async Task<FindRestaurantsResult> FindNearbyRestaurantsAsync(string apiKey, TimeSpan timeout)
        {
            var restaurants = new List<Restaurant>();

            var position = _geoCoordinateService.GetGurrentPosition(timeout);
            if (position != null)
            {
                try
                {
                    var gourmetSearchResult = await _gourmetService.SearchGourmetAsync(apiKey, position.Latitude, position.Longitude);
                    foreach (var shop in gourmetSearchResult.Results.Shops)
                    {
                        restaurants.Add(
                            new Restaurant
                            {
                                Name = shop.Name,
                                Genre = shop.Genre.Name
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
