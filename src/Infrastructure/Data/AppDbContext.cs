using DeveloperStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace DeveloperStore.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Branch> Branches { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Branch configuration
        modelBuilder.Entity<Branch>()
            .HasKey(b => b.Id);

        // Customer configuration
        modelBuilder.Entity<Customer>()
            .HasKey(c => c.Id);

        // Product configuration
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Product>()
            .OwnsOne(p => p.Price);

        // Sale configuration
        modelBuilder.Entity<Sale>()
            .HasKey(s => s.Id);
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Customer)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.CustomerId);
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Branch)
            .WithMany(b => b.Sales)
            .HasForeignKey(s => s.BranchId);
        modelBuilder.Entity<Sale>()
            .OwnsOne(s => s.TotalAmount);

        // SaleItem configuration
        modelBuilder.Entity<SaleItem>()
            .HasKey(si => si.Id);
        modelBuilder.Entity<SaleItem>()
            .HasOne(si => si.Sale)
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SaleId);
        modelBuilder.Entity<SaleItem>()
            .HasOne(si => si.Product)
            .WithMany()
            .HasForeignKey(si => si.ProductId);
        modelBuilder.Entity<SaleItem>()
            .OwnsOne(si => si.UnitPrice);
        modelBuilder.Entity<SaleItem>()
            .OwnsOne(si => si.Discount);
        modelBuilder.Entity<SaleItem>()
            .OwnsOne(si => si.TotalPrice);

        // Value object configurations for Money and Discount
        modelBuilder.ApplyUtcDateTimeConverter();
    }
}

// Extension method to handle DateTime as UTC in EF Core
public static class ModelBuilderExtensions
{
    public static void ApplyUtcDateTimeConverter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                        v => v,
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                }
            }
        }
    }
}

