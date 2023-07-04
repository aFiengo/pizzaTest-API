using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models.Base;

namespace Truextend.PizzaTest.Data.Models
{
    public class PizzaPrice 
    {
        public Guid PizzaId { get; set; }
        public Guid SizeId { get; set; }
        public int Price { get; set; }
        public Pizza Pizza { get; set; }
        public Size Size { get; set; }
    }
}
