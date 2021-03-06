﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace HatPepper.Integrations
{
    /// <summary>
    /// グルメサーチAPIクライアント
    /// </summary>
    public class GourmetService : IGourmetService
    {
        private const string GourmetSearchApiEndpoint = "https://webservice.recruit.co.jp/hotpepper/gourmet/v1/";

        /// <summary>
        /// 指定座標周辺の店舗情報を取得する
        /// </summary>
        /// <param name="hotPepperApiKey"></param>
        /// <param name="currentLocation"></param>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        public async Task<IEnumerable<GourmetInfo>> SearchGourmetInfosAsync(string hotPepperApiKey, Location currentLocation)
        {
            // リクルート WEBサービスのグルメサーチAPIを利用し、周辺のレストラン情報を取得する
            // Web APIを呼び出しJSONで結果を取得した後、Json.NETを利用してオブジェクト化する
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(
                    $"{GourmetSearchApiEndpoint}" +
                    $"?key={hotPepperApiKey}" +
                    $"&lat={currentLocation.Latitude}" +
                    $"&lng={currentLocation.Longitude}" +
                    $"&format=json&type=lite");
                var result = JsonConvert.DeserializeObject<GourmetSearchResult>(json);
                return result.Results.Shops.Select(shop => new GourmetInfo
                {
                    ShopName = shop.Name,
                    Genre = shop.Genre.Name
                });
            }
        }
    }
}
