using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IList<Restaurant>> FindNearbyRestaurantsAsync(TimeSpan timeout)
        {
            var restaurants = new List<Restaurant>();

            var position = _geoCoordinateService.GetGurrentPosition(timeout);
            var result = await _gourmetService.SearchGourmetAsync(position);
            foreach (var shop in result.Results.Shops)
            {
                restaurants.Add(
                    new Restaurant
                    {
                        Name = shop.Name,
                        Genre = shop.Genre.Name
                    });
            }

            return restaurants;
        }
    }
}
