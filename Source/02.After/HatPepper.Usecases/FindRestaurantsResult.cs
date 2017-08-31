using System.Collections.Generic;

namespace HatPepper.Usecases
{
    public class FindRestaurantsResult
    {
        public FindRestaurantsResultStatus Status { get; set; }

        public IList<Restaurant> Restaurants { get; set; }
    }
}
