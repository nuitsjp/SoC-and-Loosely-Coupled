using System.Device.Location;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;

namespace HotPepper.Console
{
    class Program
    {
        private const string GourmetSearchUri = "http://webservice.recruit.co.jp/hotpepper/gourmet/v1/?key=" + Secrets.HotPepperApiKey + "&lat={0}&lng={1}&format=json";

        static void Main(string[] args)
        {
            if (Secrets.HotPepperApiKey.StartsWith("Your"))
            {
                System.Console.WriteLine("Secrets.csファイルのHotPepperApiKeyに正しいAPIキーを設定してください。");
                System.Console.WriteLine("キーはつぎのサイトから申請して取得することができます。");
                System.Console.WriteLine("https://webservice.recruit.co.jp/register/index.html");
            }
            else
            {
                var watcher = new GeoCoordinateWatcher();
                GeoPosition<GeoCoordinate> position = null;
                Monitor.Enter(watcher);
                try
                {
                    watcher.PositionChanged += (sender, eventArgs) =>
                    {
                        Monitor.Enter(watcher);
                        try
                        {
                            position = eventArgs.Position;
                            Monitor.PulseAll(watcher);
                        }
                        finally
                        {
                            Monitor.Exit(watcher);
                        }
                    };
                    watcher.Start();
                    Monitor.Wait(watcher);
                }
                finally
                {
                    Monitor.Exit(watcher);
                }
                watcher.Stop();

                string json = null;
                using (var httpClient = new HttpClient())
                {
                    var task = httpClient.GetStringAsync(string.Format(GourmetSearchUri, position.Location.Latitude,
                        position.Location.Longitude));
                    task.Wait();
                    json = task.Result;
                }

                var root = JsonConvert.DeserializeObject<Rootobject>(json);
                System.Console.WriteLine("Powered by ホットペッパー Webサービス");
                foreach (var shop in root.results.shop)
                {
                    System.Console.WriteLine($"店舗名：{shop.name}\tジャンル：{shop.genre.name}");
                }
            }
            System.Console.WriteLine("Enterキーを押してアプリを終了してください。");
            System.Console.ReadLine();
        }
    }


    public class Rootobject
    {
        public Results results { get; set; }
    }

    public class Results
    {
        public string api_version { get; set; }
        public string results_returned { get; set; }
        public int results_start { get; set; }
        public Shop[] shop { get; set; }
        public int results_available { get; set; }
    }

    public class Shop
    {
        public string name_kana { get; set; }
        public string other_memo { get; set; }
        public Photo photo { get; set; }
        public Large_Area large_area { get; set; }
        public string sommelier { get; set; }
        public string party_capacity { get; set; }
        public Large_Service_Area large_service_area { get; set; }
        public string address { get; set; }
        public string mobile_access { get; set; }
        public string id { get; set; }
        public string lng { get; set; }
        public string course { get; set; }
        public string show { get; set; }
        public string parking { get; set; }
        public string non_smoking { get; set; }
        public string name { get; set; }
        public string horigotatsu { get; set; }
        public Genre genre { get; set; }
        public string open { get; set; }
        public string tatami { get; set; }
        public string card { get; set; }
        public string charter { get; set; }
        public string wifi { get; set; }
        public Sub_Genre sub_genre { get; set; }
        public string equipment { get; set; }
        public string shop_detail_memo { get; set; }
        public string band { get; set; }
        public Middle_Area middle_area { get; set; }
        public string lat { get; set; }
        public string karaoke { get; set; }
        public string logo_image { get; set; }
        public Urls urls { get; set; }
        public Budget budget { get; set; }
        public string midnight { get; set; }
        public string lunch { get; set; }
        public string english { get; set; }
        public string close { get; set; }
        public Service_Area service_area { get; set; }
        public string budget_memo { get; set; }
        public string tv { get; set; }
        public string private_room { get; set; }
        public Coupon_Urls coupon_urls { get; set; }
        public Small_Area small_area { get; set; }
        public Sub_Food sub_food { get; set; }
        public string barrier_free { get; set; }
        public string wedding { get; set; }
        public string access { get; set; }
        public string ktai { get; set; }
        public string child { get; set; }
        public string capacity { get; set; }
        public string open_air { get; set; }
        public string pet { get; set; }
        public string ktai_coupon { get; set; }
        public Food food { get; set; }
        public string free_food { get; set; }
        public string station_name { get; set; }
        public string _catch { get; set; }
        public string free_drink { get; set; }
    }

    public class Photo
    {
        public Pc pc { get; set; }
        public Mobile mobile { get; set; }
    }

    public class Pc
    {
        public string l { get; set; }
        public string m { get; set; }
        public string s { get; set; }
    }

    public class Mobile
    {
        public string l { get; set; }
        public string s { get; set; }
    }

    public class Large_Area
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Large_Service_Area
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Genre
    {
        public string name { get; set; }
        public string _catch { get; set; }
        public string code { get; set; }
    }

    public class Sub_Genre
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Middle_Area
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Urls
    {
        public string qr { get; set; }
        public string pc { get; set; }
        public string mobile { get; set; }
    }

    public class Budget
    {
        public string average { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Service_Area
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Coupon_Urls
    {
        public string sp { get; set; }
        public string qr { get; set; }
        public string pc { get; set; }
        public string mobile { get; set; }
    }

    public class Small_Area
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Sub_Food
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Food
    {
        public string name { get; set; }
        public string code { get; set; }
    }

}
