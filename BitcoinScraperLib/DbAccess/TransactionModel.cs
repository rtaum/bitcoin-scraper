using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitcoinScraperLib.DbAccess
{
    [Table("Transaction")]
    public class TransactionModel
    {
        [Key]
        public int Id { get; set; }

        public int BlockId { get; set; }

        public string Hash { get; set; }
    }
}
