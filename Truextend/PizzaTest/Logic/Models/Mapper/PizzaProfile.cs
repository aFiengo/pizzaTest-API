using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Truextend.PizzaTest.Data.Models;

namespace Truextend.PizzaTest.Logic.Models.Mapper
{
    public class PizzaProfile : Profile
    {
        public PizzaProfile() 
        {
            this.CreateMap<Pizza, PizzaDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Toppings, opt => opt.MapFrom(src => src.PizzaToppings.Select(pt => pt.Topping.Name)));

        }
    }
}
