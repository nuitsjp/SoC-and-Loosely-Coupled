using System.IO;
using System.Threading.Tasks;

namespace HatPepper.Presantations
{
    public interface IRestauranListConsole
    {
        Task BrowseRestaurantList(string apiKey, TextWriter textWriter);
    }
}