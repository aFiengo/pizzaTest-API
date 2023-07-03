using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truextend.PizzaTest.Data.Models
{
    public class PizzaToppings
    {
        public Guid PizzaId { get; set; }
        public Guid ToppingId { get; set; }
        public Pizza Pizza { get; set; }
        public Topping Topping { get; set; }
    }
}
