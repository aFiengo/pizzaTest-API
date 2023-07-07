using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models.Base;

namespace Truextend.PizzaTest.Data.Models
{
    public class Pizza : Entity
    {
        public string Name { get; set; }
        public string SmallImageUrl { get; set; }
        public string LargeImageUrl { get; set; }
        public virtual ICollection<Topping> Toppings { get; set; }
    }
}
