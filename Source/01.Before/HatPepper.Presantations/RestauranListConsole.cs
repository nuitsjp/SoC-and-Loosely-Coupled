﻿using System;
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
            FindRestaurants findRestaurants = new FindRestaurants();
            var result = await findRestaurants.FindNearbyRestaurantsAsync(apiKey, timeout);

            foreach (var shop in result.Results.Shops)
            {
                textWriter.WriteLine($"店舗名：{shop.Name}\tジャンル：{shop.Genre.Name}");
            }
        }
    }
}
