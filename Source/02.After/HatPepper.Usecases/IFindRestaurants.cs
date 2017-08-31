using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HatPepper.Usecases
{
    public interface IFindRestaurants
    {
        Task<FindRestaurantsResult> FindNearbyRestaurantsAsync(string apiKey, TimeSpan timeout);
    }
}