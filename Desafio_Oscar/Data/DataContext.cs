using Desafio_Oscar.Models;

namespace Desafio_Oscar.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) => Database.EnsureCreated();

        public DbSet<VeryBigSum> VeryBigSums { get; set; }
        public DbSet<DiagonalSum> DiagonalSums { get; set; }
        public DbSet<Ratio> Ratios { get; set; }

    }
}
