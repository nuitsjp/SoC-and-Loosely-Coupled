using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotPepper.Console.Integrations
{
    public interface IGourmetService
    {
        Task<IList<Shop>> SearchShopsAsync(string apiKey, double latitude, double longitude);
    }
}