using System;
using System.Threading.Tasks;

namespace BitcoinScraperConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            BitcoinDataPipeline pipeline = new BitcoinDataPipeline();
            await pipeline.StartProcessingAsync(1);
            await pipeline.CompleteAsync();
            Console.WriteLine("Loading the bitcoin data has finished");
        }
    }
}
