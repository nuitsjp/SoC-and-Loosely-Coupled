using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HotPepper.Console.Integrations
{
    public class GourmetService : IGourmetService
    {
        private const string GourmetSearchApiEndpoint = "https://webservice.recruit.co.jp/hotpepper/gourmet/v1/";

        public async Task<IList<Shop>> SearchShopsAsync(string apiKey, double latitude, double longitude)
        {
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(
                    $"{GourmetSearchApiEndpoint}" +
                    $"?key={apiKey}" +
                    $"&lat={latitude}" +
                    $"&lng={longitude}" +
                    $"&format=json&type=lite");
                var result = JObject.Parse(json);
                var shops = new List<Shop>();
                foreach (var shop in result["results"]["shop"])
                {
                    shops.Add(
                        new Shop
                        {
                            Name = (string)shop["name"],
                            Genre = (string)shop["genre"]["name"]
                        });
                }
                return shops;
            }
        }
    }
}
