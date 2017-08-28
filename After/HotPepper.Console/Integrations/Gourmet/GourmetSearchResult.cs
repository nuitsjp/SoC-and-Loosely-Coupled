using Newtonsoft.Json;

namespace HotPepper.Console.Integrations.Gourmet
{
    public class GourmetSearchResult
    {
        [JsonProperty("results")]
        public Results Results { get; set; }
    }

    public class Results
    {
        [JsonProperty("results_start")]
        public int ResultsStart { get; set; }
        [JsonProperty("results_returned")]
        public string ResultsReturned { get; set; }
        [JsonProperty("api_version")]
        public string ApiVersion { get; set; }
        [JsonProperty("shop")]
        public Shop[] Shops { get; set; }
        [JsonProperty("results_available")]
        public int ResultsAvailable { get; set; }
    }

    public class Shop
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("genre")]
        public Genre Genre { get; set; }
    }

    public class Genre
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
