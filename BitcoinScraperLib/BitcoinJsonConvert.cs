using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinScraperLib
{
    internal class BitcoinJsonConvert
    {
        public int GetBlocksCount(string content)
        {
            var parsedResponse = JObject.Parse(content);
            return parsedResponse["info"].Value<int>("blocks");
        }

        public string GetBlocksHash(string content)
        {
            var parsedResponse = JObject.Parse(content);
            return parsedResponse.Value<string>("blockHash");
        }

        public IEnumerable<string> GetTransactionHashes(string content)
        {
            var parsedResponse = JObject.Parse(content);
            return parsedResponse.GetValue("tx").
                Children().
                Select(t => t.Value<string>());
        }

        public Transaction GetTransaction(string content)
        {
            return JsonConvert.DeserializeObject<Transaction>(content);
        }
    }
}
