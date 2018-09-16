using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitcoinScraperLib
{
    public class BitcoinDataProvider
    {
        private readonly BitcoinHttpClient _bitcoinHttpClient;
        private readonly BitcoinJsonConvert _bitcoinJsonConvert;

        public BitcoinDataProvider()
        {
            // Use DI in full version :)
            _bitcoinHttpClient = new BitcoinHttpClient();
            _bitcoinJsonConvert = new BitcoinJsonConvert();
        }

        public async Task<int> GetBlocksCount()
        {
            var countContent = await _bitcoinHttpClient.GetBlocksCountContent();
            return _bitcoinJsonConvert.GetBlocksCount(countContent);
        }

        public async Task<string> GetBlocksHash(int index)
        {
            var content = await _bitcoinHttpClient.GetBlocksHashContent(index);
            return _bitcoinJsonConvert.GetBlocksHash(content);
        }

        public async Task<IEnumerable<string>> GetTransactionHashes(string hash)
        {
            var content = await _bitcoinHttpClient.GetTransactionHashesContent(hash);
            return _bitcoinJsonConvert.GetTransactionHashes(content);
        }

        public async Task<Transaction> GetTransaction(string transactionHash)
        {
            var transactionDetails = await _bitcoinHttpClient.GetTransactionContent(transactionHash);
            return _bitcoinJsonConvert.GetTransaction(transactionDetails);
        }
    }
}
