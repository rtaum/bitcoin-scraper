using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitcoinScraperLib.DbAccess
{
    [Table("Block")]
    public class BlockModel
    {
        [Key]
        public int Id { get; set; }
        public int Index { get; set; }
        public string Hash { get; set; }
    }
}
