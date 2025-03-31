using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.ValueObjects;
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

        SetAllLowercase(modelBuilder);

        // Branch configuration
        #region Branch configuration        

        modelBuilder.Entity<Branch>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<Branch>()
            .Property(b => b.Id)
            .ValueGeneratedNever();

        SeedBranchesData(modelBuilder);

        #endregion

        #region Customer configuration
        
        modelBuilder.Entity<Customer>()
            .HasKey(c => c.Id);
        modelBuilder.Entity<Customer>()
            .Property(c => c.Id)
            .ValueGeneratedNever();

        SeedCustomersData(modelBuilder);
        
        #endregion


        #region Product configuration
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);
        modelBuilder.Entity<Product>()
            .Property(p => p.Id)
            .ValueGeneratedNever();
   
        modelBuilder.Entity<Product>(builder =>
        {
            builder.OwnsOne(e => e.Rating, rating =>
            {
                rating.Property(r => r.Rate).IsRequired();
                rating.Property(r => r.Count).IsRequired();
            });
        });

        #endregion

        #region  Sale configuration
        modelBuilder.Entity<Sale>()
            .HasKey(s => s.Id);
        modelBuilder.Entity<Sale>()
            .Property(s => s.Id)
            .ValueGeneratedNever();
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Customer)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.CustomerId);
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Branch)
            .WithMany(b => b.Sales)
            .HasForeignKey(s => s.BranchId);
        

        #endregion
        
        #region  SaleItem configuration
        modelBuilder.Entity<SaleItem>()
            .HasKey(si => si.Id);
        modelBuilder.Entity<SaleItem>()
            .Property(si => si.Id)
            .ValueGeneratedNever(); 
        modelBuilder.Entity<SaleItem>()
            .HasOne(si => si.Sale)
            .WithMany(s => s.Items)
            .HasForeignKey(si => si.SaleId);
        modelBuilder.Entity<SaleItem>()
            .HasOne(si => si.Product)
            .WithMany()
            .HasForeignKey(si => si.ProductId);
        
        modelBuilder.Entity<SaleItem>()
            .OwnsOne(si => si.Discount);
        

        #endregion

        // Value object configurations for Money and Discount
        modelBuilder.ApplyUtcDateTimeConverter();
    }

    private void SetAllLowercase(ModelBuilder modelBuilder)
    {
        // Configurar nomes de tabelas e colunas para lowercase
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Configurar o nome da tabela em lowercase
            entity.SetTableName(entity.GetTableName()?.ToLower());

            // Configurar os nomes das colunas em lowercase
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName()?.ToLower());
            }

            // Configurar os nomes das chaves em lowercase
            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName()?.ToLower());
            }

            // Configurar os nomes dos índices em lowercase
            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()?.ToLower());
            }

            // Configurar os nomes das chaves estrangeiras em lowercase
            foreach (var foreignKey in entity.GetForeignKeys())
            {
                foreignKey.SetConstraintName(foreignKey.GetConstraintName()?.ToLower());
            }
        }
    }

    private void SeedBranchesData(ModelBuilder modelBuilder)
    {
        // Seed data
        modelBuilder.Entity<Branch>().HasData(
            new Branch(1, "Main Branch", "123 Main St", "New York", "NY", "10001", "123-456-7890"),
            new Branch(2, "Secondary Branch", "456 Elm St", "Los Angeles", "CA", "90001", "987-654-3210")
        );
    }

    private void SeedCustomersData(ModelBuilder modelBuilder)
    {
        // Seed data
        modelBuilder.Entity<Customer>().HasData(
            new Customer(1, "John", "Doe", "john.doe@example.com", "123-456-7890"),
            new Customer(2, "Jane", "Smith", "jane.smith@example.com", "987-654-3210"),
            new Customer(3, "Alice", "Johnson", "alice.johnson@example.com", "555-123-4567")            
        );
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

