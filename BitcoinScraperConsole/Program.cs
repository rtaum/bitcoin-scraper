using BitcoinScraperLib;
using System;
using System.Threading.Tasks;

namespace BitcoinScraperConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var bitcoinClient = new BitcoinDataProvider();
            var count = await bitcoinClient.GetBlocksCount();
            var hash = await bitcoinClient.GetBlocksHash(count - 1);
            var transactionHashes = await bitcoinClient.GetTransactionHashes(hash);
            var transactions = await bitcoinClient.GetTransactions(transactionHashes);
            Console.WriteLine("Hello World!");
        }
    }
}
