using Newtonsoft.Json;

namespace HatPepper.Integrations
{
    public class Shop
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("genre")]
        public Genre Genre { get; set; }
    }
}
