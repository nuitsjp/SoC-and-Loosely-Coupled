using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using HatPepper.Integrations;
using HatPepper.Presantations;
using HatPepper.Usecases;

namespace HatPepper
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Powered by ホットペッパー Webサービス");
            Console.WriteLine();

            if (Secrets.HotPepperApiKey.StartsWith("Your"))
            {
                Console.WriteLine("Secrets.csファイルのHotPepperApiKeyに正しいAPIキーを設定してください。");
                Console.WriteLine("キーはつぎのサイトから申請して取得することができます。");
                Console.WriteLine("https://webservice.recruit.co.jp/register/index.html");
            }
            else
            {
                // DIコンテナーを初期化する
                var builder = new ContainerBuilder();
                builder.RegisterType<FindRestaurants>().As<IFindRestaurants>();
                builder.RegisterType<GeoCoordinator>().As<IGeoCoordinator>();
                builder.RegisterType<GourmetService>().As<IGourmetService>();
                builder.RegisterType<RestauranListConsole>();
                var container = builder.Build();


                // コンテナからRestauranListConsoleを取得する
                var restauranListConsole = container.Resolve<RestauranListConsole>();

                try
                {
                    // レストラン一覧を出力する
                    await restauranListConsole.BrowseRestaurantList(Console.Out, Secrets.HotPepperApiKey, TimeSpan.FromMinutes(1));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine();
            Console.WriteLine("Enterキーを押してアプリを終了してください。");
            Console.ReadLine();

        }
    }
}
