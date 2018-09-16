using Newtonsoft.Json;

namespace BitcoinScraperLib
{
    public class VIn
    {
        [JsonProperty("txid")]
        public string TransactionHash { get; set; }

        [JsonProperty("addr")]
        public string Address { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }
    }
}