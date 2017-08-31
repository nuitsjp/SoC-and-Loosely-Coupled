using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HatPepper.Integrations
{
    internal class Results
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
}
