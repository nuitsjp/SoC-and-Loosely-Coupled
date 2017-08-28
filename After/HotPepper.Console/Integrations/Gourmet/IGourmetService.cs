using System.Threading.Tasks;
using HotPepper.Console.Integrations.GeoCoordinate;

namespace HotPepper.Console.Integrations.Gourmet
{
    public interface IGourmetService
    {
        Task<GourmetSearchResult> SearchGourmetAsync(string apiKey, double latitude, double longitude);
    }
}