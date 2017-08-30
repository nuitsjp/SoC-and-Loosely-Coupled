using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nuits.System.Device.Location;

namespace HotPepper.Console
{
    class Program
    {
        private const string GourmetSearchApiEndpoint = "https://webservice.recruit.co.jp/hotpepper/gourmet/v1/";

        static async Task Main()
        {
            if (Secrets.HotPepperApiKey.StartsWith("Your"))
            {
                System.Console.WriteLine("Secrets.csファイルのHotPepperApiKeyに正しいAPIキーを設定してください。");
                System.Console.WriteLine("キーはつぎのサイトから申請して取得することができます。");
                System.Console.WriteLine("https://webservice.recruit.co.jp/register/index.html");
            }
            else
            {
                System.Console.WriteLine("Powered by ホットペッパー Webサービス");
                System.Console.WriteLine();

                var hotPepperApiKey = Secrets.HotPepperApiKey;
                var timeout = TimeSpan.FromMinutes(1);

                // GeoCoordinatorはSystem.Device.Locator.GeoCoordinateWatcherをラップしたクラス
                // GeoCoordinateWatcherは位置が取得できたことをイベントで受け取る必要があり
                // ここに直接埋め込むと、本筋と離れたところで複雑になってしまうため
                // 今回はNuits.System.Device.Locationとして事前に分離済み。
                // 詳細はLibraryフォルダの下を参照。
                var geoCoordinator = new GeoCoordinator();
                var currentLocation = geoCoordinator.GetCurrent(timeout);

                // リクルート WEBサービスのグルメサーチAPIを利用し、周辺のレストラン情報を取得する
                // Web APIを呼び出しJSONで結果を取得した後、Json.NETを利用してオブジェクト化する
                JObject result;
                using (var httpClient = new HttpClient())
                {
                    var json = await httpClient.GetStringAsync(
                        $"{GourmetSearchApiEndpoint}" +
                        $"?key={hotPepperApiKey}" +
                        $"&lat={currentLocation.Latitude}" +
                        $"&lng={currentLocation.Longitude}" +
                        $"&format=json&type=lite");
                    result = JObject.Parse(json);
                }

                // 取得結果を出力する
                foreach (var shop in result["results"]["shop"])
                {
                    System.Console.WriteLine($"店舗名：{(string)shop["name"]}\tジャンル：{(string)shop["genre"]["name"]}");
                }
            }
            System.Console.WriteLine();
            System.Console.WriteLine("Enterキーを押してアプリを終了してください。");
            System.Console.ReadLine();
        }
    }
}
