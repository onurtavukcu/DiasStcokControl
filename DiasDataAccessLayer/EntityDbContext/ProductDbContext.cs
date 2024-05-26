using DiasShared.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DiasDataAccessLayer.EntityDbContext
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<PriceHistory> PricesHistory { get; set; }
        public virtual DbSet<PriceType> PriceTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            optionsBuilder
                .UseNpgsql
                (@"Server=localhost;Port=5432; Database=DiasStockControlDB; User Id=postgres ;Password=1123581321",
                    b => b.MigrationsAssembly("DiasDataAccessLayer"));

            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>(entity =>
            {
                entity.HasMany(i => i.PriceHistories)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);
            });

            builder.Entity<PriceHistory>(entity =>
            {
                entity.HasOne(ph => ph.PriceType)
                .WithMany(pt => pt.PriceHistories)
                .HasForeignKey(ph => ph.PriceTypeId);
            });

            builder.Entity<PriceType>()
                .Property(pt => pt.PriceTypes)
                .HasConversion<int>();

            builder.Entity<PriceType>()
                .HasData(
                new PriceType
                {
                    id = 1,
                    PriceTypes = PriceTypeEnums.Old,
                },
                new PriceType
                {
                    id = 2,
                    PriceTypes = PriceTypeEnums.Current,
                }
                );
        }
    }
}
