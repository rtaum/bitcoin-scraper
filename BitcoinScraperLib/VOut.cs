using Newtonsoft.Json;

namespace BitcoinScraperLib
{
    public class VOut
    {
        [JsonProperty("spentTxId")]
        public string TransactionHash { get; set; }

        [JsonProperty("scriptPubKey")]
        public ScriptPubKey ScriptPubKey { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }
    }
}