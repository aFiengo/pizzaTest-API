using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Configuration.Models;

namespace Truextend.PizzaTest.Configuration
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        private readonly IConfiguration _configuration;
        public ApplicationConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DatabaseConnectionString GetDatabaseConnectionString()
        {
            return new DatabaseConnectionString()
            {
                DATABASE = _configuration.GetSection("ConnectionStrings").GetSection("PizzaDbConnection").Value
            };
        }
        public PizzaDefaultSettings GetPizzaDefaultSettings()
        {
            return new PizzaDefaultSettings()
            {
                DefaultSmallImageUrl = _configuration.GetSection("PizzaDefaultSettings").GetSection("DefaultSmallImageUrl").Value,
                DefaultLargeImageUrl = _configuration.GetSection("PizzaDefaultSettings").GetSection("DefaultLargeImageUrl").Value
            };
        }

    }

}
