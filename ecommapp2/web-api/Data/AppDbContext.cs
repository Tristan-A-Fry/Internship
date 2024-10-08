using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommapp.Models;
using Microsoft.EntityFrameworkCore;


/*
Purpose of AppDbContext.cs
    AppDbContext.cs is a class that defines the database context for Entity Framework Core. A DbContext is the bridge between your domain or entity classes and the database. 
    It is responsible for querying and saving data to the database and manages the configuration of entity classes mapped to the database tables.

What AppDbContext.cs is Doing
Inherits from DbContext:
    The AppDbContext class inherits from the DbContext class provided by Entity Framework Core. This inheritance allows AppDbContext to leverage all the functionality provided by 
    DbContext, such as querying, saving, and configuring the database.

Constructor with DbContextOptions: public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    The constructor takes an instance of DbContextOptions<AppDbContext> as a parameter and passes it to the base class constructor. This allows you to configure the context, 
    such as specifying the database provider (e.g., SQL Server, InMemory).
*/

namespace ecommapp.Data
{
    public class AppDbContext :DbContext 
    {
       public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){} 

       public DbSet<ProductCategory> ProductCategories {get; set;}
       public DbSet<Supplier> Suppliers {get; set;}
       public DbSet<Customer> Customers {get; set;}
       public DbSet<Order> Orders {get; set;}
       public DbSet<OrderItem> OrderItems {get; set;}
       public DbSet<Product> Products {get; set;}
       public DbSet<User> Users {get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductCategory)
            .WithMany(pc => pc.Products)
            .HasForeignKey(p => p.ProductCategoryId);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Supplier)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey("CustomerId");
    }
    }
}

