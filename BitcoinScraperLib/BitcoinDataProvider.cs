using System.Collections.Generic;
using System.Linq;
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
            var content = await _bitcoinHttpClient.GetTransactionContent(hash);
            return _bitcoinJsonConvert.GetTransactionHashes(content);
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(IEnumerable<string> hashes)
        {
            var transactions = hashes.Select(async h =>
            {
                var hashContent = await _bitcoinHttpClient.GetTransactionContent(h);
                return _bitcoinJsonConvert.GetTransaction(hashContent);
            });

            return await Task.WhenAll(transactions);
        }
    }
}
