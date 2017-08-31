﻿using Newtonsoft.Json;

namespace HatPepper.Integrations
{
    internal class Shop
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("genre")]
        public Genre Genre { get; set; }
    }
}
