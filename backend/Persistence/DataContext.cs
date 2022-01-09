using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using System;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    public class DataContext : DbContext, IApplicationDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<FilterAttribute> Attributes { get; set; }
        public DbSet<Product_FilterAttribute> Product_FilterAttribute { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product_Category> Product_Category { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>().HasIndex(b => b.name);

            builder.Entity<Product_FilterAttribute>(x => x.HasKey(ua => new { ua.FilterAttributeid, ua.Productid }));

            builder.Entity<Product_FilterAttribute>(x => x.HasOne(a => a.Product)
            .WithMany(b => b.product_FilterAttribute)
            .HasForeignKey(a => a.Productid));

            builder.Entity<Product_FilterAttribute>(x => x.HasOne(a => a.FilterAttribute)
            .WithMany(b => b.product_FilterAttribute)
            .HasForeignKey(a => a.FilterAttributeid));


            builder.Entity<Product_Category>(x => x.HasKey(ua => new { ua.categoryid, ua.productid }));

            builder.Entity<Product_Category>(x => x.HasOne(a => a.product)
            .WithMany(b => b.product_Category)
            .HasForeignKey(a => a.productid));

            builder.Entity<Product_Category>(x => x.HasOne(a => a.category)
            .WithMany(b => b.product_Category)
            .HasForeignKey(a => a.categoryid));

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
    }

}