using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models.Base;

namespace Truextend.PizzaTest.Data.Models
{
    public class OrderInfo : Entity
    {
        public DateTime OrderDate { get; set; }   
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public int UserPhone { get; set; }
        public string UserAddress { get; set; }
        public string UserEmail { get; set; }
        public Guid PizzaId { get; set; }
        public Pizza Pizza { get; set; }
        public bool ExtraTopping { get; set; }
        public Guid ToppingId { get; set; }
        public Topping Topping { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }
}
