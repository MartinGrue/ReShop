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


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>().HasIndex(b => b.name);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
    }

}