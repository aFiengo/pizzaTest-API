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
    public class PizzaDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public PizzaDbContext(IConfiguration config)
        {
            _config = config;
        }
        public DbSet<Pizza> Pizza { get; set; }
        public DbSet<PizzaTopping> PizzaTopping { get; set; }
        public DbSet<Topping> Topping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL(_config["ConnectionStrings:PizzaDbConnection"]);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Pizza>()
                .HasMany(p => p.PizzaToppings)
                .WithOne(pt => pt.Pizza)
                .HasForeignKey(pt => pt.PizzaId);

            modelBuilder.Entity<Topping>()
                .HasMany(t => t.PizzaToppings)
                .WithOne(pt => pt.Topping)
                .HasForeignKey(pt => pt.ToppingId);

            modelBuilder.Entity<PizzaTopping>()
                .HasKey(pt => new { pt.PizzaId, pt.ToppingId });

            modelBuilder.Entity<PizzaTopping>()
                .HasOne(pt => pt.Pizza)
                .WithMany(p => p.PizzaToppings)
                .HasForeignKey(pt => pt.PizzaId);

            modelBuilder.Entity<PizzaTopping>()
                .HasOne(pt => pt.Topping)
                .WithMany(t => t.PizzaToppings)
                .HasForeignKey(pt => pt.ToppingId);

        }
    }
}
