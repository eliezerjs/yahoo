using Microsoft.EntityFrameworkCore;
using WebYahoo.Models;

namespace StockService.Data
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }
        public DbSet<PriceVariation> PriceVariations { get; set; }
    }
}