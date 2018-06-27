using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MountainAddicted.Database
{
    public class MountainDbContext : DbContext
    {
        public MountainDbContext(string connectionString) 
            : base(connectionString)
        {
        }

        public MountainDbContext() 
            : this("MountainDBConnectionString")
        {
        }

        public DbSet<HeightRequestDbData> HeightRequests { get; set; }
        public DbSet<MountainDbData> Mountains { get; set; }
        public DbSet<GCodeConfig> GCodeConfigs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MountainDbData>().Property(i => i.NeLat).HasPrecision(14, 10);
            modelBuilder.Entity<MountainDbData>().Property(i => i.NeLng).HasPrecision(14, 10);
            modelBuilder.Entity<MountainDbData>().Property(i => i.SwLat).HasPrecision(14, 10);
            modelBuilder.Entity<MountainDbData>().Property(i => i.SwLng).HasPrecision(14, 10);

            modelBuilder.Entity<HeightRequestDbData>().Property(i => i.NeLat).HasPrecision(14, 10);
            modelBuilder.Entity<HeightRequestDbData>().Property(i => i.NeLng).HasPrecision(14, 10);
            modelBuilder.Entity<HeightRequestDbData>().Property(i => i.SwLat).HasPrecision(14, 10);
            modelBuilder.Entity<HeightRequestDbData>().Property(i => i.SwLng).HasPrecision(14, 10);

            base.OnModelCreating(modelBuilder);
        }

    }
}
