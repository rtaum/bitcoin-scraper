using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace BitcoinScraperLib
{
    public class BitcoinJsonConvert
    {
        public int GetBlocksCount(string content)
        {
            return ParseValue<int>(content, "blocks");
        }

        public string GetBlocksHash(string content)
        {
            return ParseValue<string>(content, "blockHash");
        }

        public IEnumerable<string> GetTransactionHashes(string content)
        {
            return ParseValues<string>(content, "tx");
        }

        public Transaction GetTransaction(string content)
        {
            return JsonConvert.DeserializeObject<Transaction>(content);
        }

        private T ParseValue<T>(string responseData, string propertyName)
        {
            var parsedResponse = JObject.Parse(responseData);
            return parsedResponse.Value<T>(propertyName);
        }

        private IEnumerable<T> ParseValues<T>(string responseData, string propertyName)
        {
            var parsedResponse = JObject.Parse(responseData);
            return parsedResponse.GetValue(propertyName).
                Children().
                Select(t => t.Value<T>());
        }
    }
}
