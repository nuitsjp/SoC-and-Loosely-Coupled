using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotPepper.Console.Usecases
{
    public interface IFindRestaurants
    {
        Task<IList<Restaurant>> FindNearbyRestaurantsAsync(TimeSpan timeout);
    }
}