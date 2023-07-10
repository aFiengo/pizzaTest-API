using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Configuration.Models;
using Truextend.PizzaTest.Data.Models;

namespace Truextend.PizzaTest.Data
{
    public class PizzaDbContext : DbContext
    {
        private readonly IApplicationConfiguration _config;
        public PizzaDbContext(IApplicationConfiguration config)
        {
            _config = config;
        }
        public DbSet<Pizza> Pizza { get; set; }

        public DbSet<Topping> Topping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL(_config.GetDatabaseConnectionString().DATABASE);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Pizza>()
            .HasMany(p => p.Toppings)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "PizzaTopping",
                j => j
                    .HasOne<Topping>()
                    .WithMany()
                    .HasForeignKey("ToppingId"),
                j => j
                    .HasOne<Pizza>()
                    .WithMany()
                    .HasForeignKey("PizzaId")
                );
        }
        public PizzaDefaultSettings DefaultSettings()
        {
            return _config.GetImgUrlString();
        }
    }
}
