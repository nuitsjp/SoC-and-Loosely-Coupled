using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HatPepper.Usecases;

namespace HatPepper.Presantations
{
    /// <summary>
    /// プレゼンテーションはテストしない前提
    /// </summary>
    public class RestauranListConsole
    {
        private readonly IFindRestaurants _findRestaurants;

        public RestauranListConsole(IFindRestaurants findRestaurants)
        {
            _findRestaurants = findRestaurants;
        }


        /// <summary>
        /// 現在地周辺のレストラン一覧を表示する
        /// </summary>
        /// <param name="textWriter"></param>
        /// <param name="apiKey"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task BrowseRestaurantList(TextWriter textWriter, string apiKey, TimeSpan timeout)
        {
            // 現在地周辺のレストラン一覧を取得する
            var result = await _findRestaurants.FindNearbyRestaurantsAsync(apiKey, timeout);

            // 取得結果を表示する
            if (result.Status == FindRestaurantsResultStatus.Ok)
            {
                foreach (var restaurant in result.Restaurants)
                {
                    textWriter.WriteLine($"店舗名：{restaurant.Name}\tジャンル：{restaurant.Name}");
                }
            }
            else
            {
                textWriter.WriteLine($"result.Status：{result.Status}");
            }
        }
    }
}
