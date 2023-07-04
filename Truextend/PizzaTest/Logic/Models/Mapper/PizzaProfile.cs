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
                .ForMember(dest => dest.Toppings, opt => opt.MapFrom(src => src.PizzaToppings.Select(pt => pt.Topping.Name)))
                .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src => src.PizzaPrices.Select(pp => pp.Size)))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PizzaPrices.Select(pp => pp.Price)));
        }
    }
}
