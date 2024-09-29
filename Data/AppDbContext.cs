using EcommerceProductManagement.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EcommerceProductManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and decimal precision
            ConfigureRelationships(modelBuilder);
            ConfigureDecimalPrecision(modelBuilder);

            // Seed data for entities
            SeedCategories(modelBuilder);
            SeedProducts(modelBuilder);
            SeedOrders(modelBuilder);
            SeedOrderItems(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            //Product and Category many-to-many relationship
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .UsingEntity(j => j.ToTable("ProductCategories"));

            //Order and OrderItem one-to-many relationship
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            //Product and OrderItem one-to-many relationship
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
        }

        private void ConfigureDecimalPrecision(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
        }

        private void SeedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic Devices" },
                new Category { Id = 2, Name = "Books", Description = "Various Books" },
                new Category { Id = 3, Name = "Clothing", Description = "Apparel and Accessories" }
            );
        }

        private void SeedProducts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "Gaming Laptop", Price = 999.99m, StockQuantity = 50 },
                new Product { Id = 2, Name = "Smartphone", Description = "Latest Model", Price = 599.99m, StockQuantity = 100 },
                new Product { Id = 3, Name = "T-shirt", Description = "Plain White T-shirt", Price = 9.99m, StockQuantity = 200 }
            );
        }

        private void SeedOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, CustomerName = "John Doe", OrderDate = DateTime.Now.AddDays(-10) },
                new Order { Id = 2, CustomerName = "Last 2Month", OrderDate = DateTime.Now.AddMonths(-2) },
                new Order { Id = 3, CustomerName = "Last 3Month", OrderDate = DateTime.Now.AddMonths(-3) },
                new Order { Id = 4, CustomerName = "Last Month Jerald", OrderDate = DateTime.Now.AddMonths(-1) },
                new Order { Id = 5, CustomerName = "Last Month Kristian", OrderDate = DateTime.Now.AddDays(-28) },
                new Order { Id = 6, CustomerName = "Last Month Samonte", OrderDate = DateTime.Now.AddDays(-20) }
            );
        }

        private void SeedOrderItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 999.99m },
                new OrderItem { Id = 2, OrderId = 1, ProductId = 2, Quantity = 1, UnitPrice = 599.99m }
            );
        }
    }
  }