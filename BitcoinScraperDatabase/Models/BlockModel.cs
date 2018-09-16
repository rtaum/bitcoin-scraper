using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitcoinScraperDatabase.Models
{
    [Table("Block")]
    public class BlockModel
    {
        [Key]
        public int Id { get; set; }

        public string Hash { get; set; }

        public int Index { get; set; }

        public virtual IList<TransactionModel> Transactions { get; set; }
    }
}
