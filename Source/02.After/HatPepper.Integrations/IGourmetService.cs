using System.Collections.Generic;
using System.Threading.Tasks;

namespace HatPepper.Integrations
{
    public interface IGourmetService
    {
        Task<IEnumerable<Shop>> SearchShopsAsync(string hotPepperApiKey, Location currentLocation);
    }
}