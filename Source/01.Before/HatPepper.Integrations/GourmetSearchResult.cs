﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HatPepper.Integrations
{
    public class GourmetSearchResult
    {
        [JsonProperty("results")]
        public Results Results { get; set; }
    }
}
