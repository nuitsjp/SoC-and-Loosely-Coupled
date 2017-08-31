using System.Collections.Generic;
using System.Threading.Tasks;

namespace HatPepper.Integrations
{
    /// <summary>
    /// グルメサーチAPIクライアント
    /// </summary>
    public interface IGourmetService
    {
        /// <summary>
        /// 指定座標周辺の店舗情報を取得する
        /// </summary>
        /// <param name="hotPepperApiKey"></param>
        /// <param name="currentLocation"></param>
        /// <returns></returns>
        /// <exception cref="System.Net.Http.HttpRequestException"></exception>
        Task<GourmetSearchResult> SearchGourmetInfosAsync(string hotPepperApiKey, Location currentLocation);
    }
}