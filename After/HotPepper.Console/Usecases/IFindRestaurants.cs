using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotPepper.Console.Usecases
{
    public interface IFindRestaurants
    {
        Task<FindRestaurantsResult> FindNearbyRestaurantsAsync(string apiKey, TimeSpan timeout);
    }
}