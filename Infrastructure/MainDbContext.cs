using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// Основной контекст базы данных
    /// </summary>
    public class MainDbContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductGroup>()
            .HasKey(pg => new { pg.ProductId, pg.GroupId });

            modelBuilder.Entity<ProductGroup>()
            .HasOne(pg => pg.Product)
            .WithMany(p => p.ProductGroups)
            .HasForeignKey(pg => pg.ProductId);

            modelBuilder.Entity<ProductGroup>()
            .HasOne(pg => pg.Group)
            .WithMany(g => g.ProductGroups)
            .HasForeignKey(pg => pg.GroupId);

            //Ограничил кол-во цифр после запятой до сотых
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18, 2)");
        }
    }
}
