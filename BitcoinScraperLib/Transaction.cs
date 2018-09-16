using Newtonsoft.Json;
using System.Collections.Generic;

namespace BitcoinScraperLib
{
    public class Transaction
    {
        [JsonProperty("txid")]
        public string Hash { get; set; }

        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }

        [JsonProperty("vin")]
        public IEnumerable<VIn> VIns { get; set; }

        [JsonProperty("vout")]
        public IEnumerable<VOut> VOuts { get; set; }
    }
}