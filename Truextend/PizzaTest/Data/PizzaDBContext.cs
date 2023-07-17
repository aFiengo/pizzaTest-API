using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Configuration;
using Truextend.PizzaTest.Configuration.Models;
using Truextend.PizzaTest.Data.Models;

namespace Truextend.PizzaTest.Data
{
    public class PizzaDbContext : DbContext
    {
        private readonly IApplicationConfiguration _applicationConfiguration;

        public PizzaDbContext(DbContextOptions<PizzaDbContext> options, IApplicationConfiguration applicationConfiguration)
            : base(options)
        {
            _applicationConfiguration = applicationConfiguration;
        }

        public DbSet<Pizza> Pizza { get; set; }

        public DbSet<Topping> Topping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(_applicationConfiguration.GetDatabaseConnectionString().DATABASE);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Pizza>()
            .HasMany(p => p.Toppings)
            .WithMany(t => t.Pizzas)
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
