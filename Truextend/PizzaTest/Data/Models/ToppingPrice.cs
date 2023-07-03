using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truextend.PizzaTest.Data.Models
{
    public class ToppingPrice
    {
        public Guid ToppingId { get; set; }
        public int Price { get; set; }
        public Topping  Topping { get; set; }
    }
}
