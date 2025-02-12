using Microsoft.EntityFrameworkCore;

namespace FuelDistanceCalculator.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        // Füge hier deine Tabellen als DbSet hinzu
        //public DbSet<TankstellenInfo> TankstellenInfos { get; set; }
    }

    public class TankstellenInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adresse { get; set; }
        public decimal Preis { get; set; }
        public DateTime Datum { get; set; }
    }
}
