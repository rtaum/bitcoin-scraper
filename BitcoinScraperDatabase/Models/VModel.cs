using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitcoinScraperDatabase.Models
{
    public abstract class VModel
    {
        [Key]
        public int Id { get; set; }

        public string Address { get; set; }

        public string TransactionHash { get; set; }

        public decimal Value { get; set; }

        [ForeignKey("TransactionId")]
        public TransactionModel Transaction { get; set; }
    }
}
