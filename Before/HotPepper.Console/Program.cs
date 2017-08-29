using System;
using System.Device.Location;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                await new Program().FindRestaurants();
            }
            System.Console.WriteLine();
            System.Console.WriteLine("Enterキーを押してアプリを終了してください。");
            System.Console.ReadLine();
        }

        private async Task FindRestaurants()
        {
            System.Console.WriteLine("Powered by ホットペッパー Webサービス");
            System.Console.WriteLine();

            var hotPepperApiKey = Secrets.HotPepperApiKey;

            // GeoCoordinateWatcherを利用して位置情報を取得する  
            // GeoCoordinateWatcherではStart直後は位置情報が取得できないため
            // 最初のPositionChangedイベントの発生を待つ必要がある
            GeoCoordinate geoCoordinate;
            var timeout = TimeSpan.FromMinutes(1);
            using (var watcher = new GeoCoordinateWatcher())
            {
                // PositionChangedイベントを監視し、イベント発生時に待機中のスレッドを再開する
                watcher.PositionChanged += (sender, eventArgs) =>
                {
                    Monitor.Enter(this);
                    try
                    {
                        Monitor.PulseAll(this);
                    }
                    finally
                    {
                        Monitor.Exit(this);
                    }
                };

                Monitor.Enter(this);
                try
                {
                    // GeoCoordinateWatcherを起動し、PositionChangedイベントが発生するか
                    // タイムアウトまで待機する
                    watcher.Start();
                    Monitor.Wait(this, timeout);
                }
                finally
                {
                    Monitor.Exit(this);
                }
                geoCoordinate = watcher.Position.Location;
                watcher.Stop();
            }

            // リクルート WEBサービスのグルメサーチAPIを利用し、周辺のレストラン情報を取得する
            // Web APIを呼び出しJSONで結果を取得した後、Json.NETを利用してオブジェクト化する
            JObject result;
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(
                    $"{GourmetSearchApiEndpoint}" +
                    $"?key={hotPepperApiKey}" +
                    $"&lat={geoCoordinate.Latitude}" +
                    $"&lng={geoCoordinate.Longitude}" +
                    $"&format=json&type=lite");
                result = JObject.Parse(json);
            }

            // 取得結果を出力する
            foreach (var shop in result["results"]["shop"])
            {
                System.Console.WriteLine($"店舗名：{(string)shop["name"]}\tジャンル：{(string)shop["genre"]["name"]}");
            }
        }
    }
}
