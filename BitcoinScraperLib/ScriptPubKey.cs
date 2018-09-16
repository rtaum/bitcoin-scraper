using Newtonsoft.Json;
using System.Collections.Generic;

namespace BitcoinScraperLib
{
    public class ScriptPubKey
    {
        [JsonProperty("addresses")]
        public IEnumerable<string> Addresses { get; set; }
    }
}