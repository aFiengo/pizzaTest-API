using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models;

namespace Truextend.PizzaTest.Data
{
    public class PizzaDBContext : DbContext
    {
        private readonly IConfiguration _config;
        public PizzaDBContext(IConfiguration config)
        {
            _config = config;
        }
        public DbSet<Pizza> Pizza { get; set; }
        public DbSet<PizzaPrice> PizzaPrice { get; set; }
        public DbSet<PizzaTopping> PizzaTopping { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Topping> Topping { get; set; }
        public DbSet<ToppingPrice> ToppingPrice { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config.GetConnectionString("PizzeriaDB"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Pizza>()
                .HasMany(p => p.PizzaToppings)
                .WithOne(pt => pt.Pizza)
                .HasForeignKey(pt => pt.PizzaId);

            modelBuilder.Entity<Pizza>()
                .HasMany(p => p.PizzaPrices)
                .WithOne(pp => pp.Pizza)
                .HasForeignKey(pp => pp.PizzaId);

            modelBuilder.Entity<Size>()
                .HasMany(s => s.PizzaPrices)
                .WithOne(pp => pp.Size)
                .HasForeignKey(pp => pp.SizeId);

            modelBuilder.Entity<PizzaTopping>()
                .HasKey(pt => new { pt.PizzaId, pt.ToppingId });

            modelBuilder.Entity<PizzaTopping>()
                .HasOne(pt => pt.Pizza)
                .WithMany(p => p.PizzaToppings)
                .HasForeignKey(pt => pt.PizzaId);

            modelBuilder.Entity<PizzaTopping>()
                .HasOne(pt => pt.Topping)
                .WithMany()
                .HasForeignKey(pt => pt.ToppingId);

            modelBuilder.Entity<OrderInfo>()
                .HasOne(o => o.Pizza)
                .WithOne()
                .HasForeignKey<OrderInfo>(o => o.PizzaId);

            modelBuilder.Entity<OrderInfo>()
                .HasOne(o => o.Topping)
                .WithMany()
                .HasForeignKey(o => o.ToppingId);

            modelBuilder.Entity<Topping>()
                .HasMany(t => t.PizzaToppings)
                .WithOne(pt => pt.Topping)
                .HasForeignKey(pt => pt.ToppingId);

            modelBuilder.Entity<Topping>()
                .HasOne(t => t.ToppingPrice)
                .WithOne()
                .HasForeignKey<Topping>(t => t.ToppingPrice);

            modelBuilder.Entity<ToppingPrice>()
                .HasKey(tp => tp.ToppingId);

            modelBuilder.Entity<ToppingPrice>()
                .HasOne(tp => tp.Topping)
                .WithOne()
                .HasForeignKey<ToppingPrice>(tp => tp.ToppingId);
        }
    }
}
