using System.Collections.Generic;
using System.Threading.Tasks;
using Nuits.System.Device.Location;

namespace HotPepper.Console.Integrations
{
    public interface IGourmetService
    {
        Task<IEnumerable<Shop>> SearchShopsAsync(string hotPepperApiKey, Location currentLocation);
    }
}