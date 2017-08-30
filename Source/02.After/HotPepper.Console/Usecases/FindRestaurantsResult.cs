using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotPepper.Console.Usecases
{
    public class FindRestaurantsResult
    {
        public FindRestaurantsResultStatus Status { get; set; }

        public IList<Restaurant> Restaurants { get; set; }
    }
}
