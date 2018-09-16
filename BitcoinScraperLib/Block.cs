using System.Collections.Generic;

namespace BitcoinScraperLib
{
    public class Block
    {
        public int Index { get; set; }

        public string Hash { get; set; }

        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
