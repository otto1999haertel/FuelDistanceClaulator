using Microsoft.EntityFrameworkCore;
using FuelDistanceCalculator.Model;

namespace FuelDistanceCalculator.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        // Füge hier deine Tabellen als DbSet hinzu
        public DbSet<TankinfoModel> TankinfoModel { get; set; }
    }
}
