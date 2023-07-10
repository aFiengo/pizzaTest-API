using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truextend.PizzaTest.Configuration.Models
{
    public interface IApplicationConfiguration
    {
        DatabaseConnectionString GetDatabaseConnectionString();
        PizzaDefaultSettings GetImgUrlString();
    }
}
