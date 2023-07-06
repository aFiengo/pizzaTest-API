using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truextend.PizzaTest.Logic.Models
{
    public class PizzaDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ToppingDTO> Toppings { get; set; }
    }

    public class PizzaNameDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public class PizzaCreateDTO
    {
        public string Name { get; set; }
        public IEnumerable<Guid> ToppingIds { get; set; }
    }
}
