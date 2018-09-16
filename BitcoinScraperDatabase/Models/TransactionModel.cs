using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitcoinScraperDatabase.Models
{
    [Table("Transaction")]
    public class TransactionModel
    {
        [Key]
        public int Id { get; set; }

        public string Hash { get; set; }

        [ForeignKey("BlockId")]
        public BlockModel Block { get; set; }

        public virtual List<VInModel> VIns { get; set; }

        public virtual IList<VOutModel> VOuts { get; set; }
    }
}
