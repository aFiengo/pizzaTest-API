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
        public PizzaDbContext(IApplicationConfiguration applicationConfiguration)
        {
            _applicationConfiguration = applicationConfiguration;
        }
        private readonly IApplicationConfiguration _applicationConfiguration;
        public DbSet<Pizza> Pizza { get; set; }

        public DbSet<Topping> Topping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL(_applicationConfiguration.GetDatabaseConnectionString().DATABASE);
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
        
    }
}
