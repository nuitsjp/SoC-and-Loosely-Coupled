using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HatPepper.Usecases;

namespace HatPepper.Presantations
{
    public class RestauranListConsole : IRestauranListConsole
    {
        private readonly IFindRestaurants _findRestaurants;

        public RestauranListConsole(IFindRestaurants findRestaurants)
        {
            _findRestaurants = findRestaurants;
        }


        public async Task BrowseRestaurantList(string apiKey, TextWriter textWriter)
        {
            // GeoCoordinateWatcherを利用して位置情報を取得する  
            // GeoCoordinateWatcherではStart直後は位置情報が取得できないため
            // 最初のPositionChangedイベントの発生を待つ必要がある
            var timeout = TimeSpan.FromMinutes(1);

            var result = await _findRestaurants.FindNearbyRestaurantsAsync(apiKey, timeout);

            // 取得結果を出力する
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
