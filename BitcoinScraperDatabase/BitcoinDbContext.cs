using BitcoinScraperDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace BitcoinScraperDatabase
{
    public class BitcoinDbContext : DbContext
    {
        public DbSet<BlockModel> Blocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\;Database=Bitcoin;Trusted_Connection=True;");
        }
    }
}
