using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models.Base;

namespace Truextend.PizzaTest.Data.Models
{
    public class Size : Entity
    {
        public string Name { get; set; }
        public List<PizzaPrice> PizzaPrices { get; set; }
    }
}
