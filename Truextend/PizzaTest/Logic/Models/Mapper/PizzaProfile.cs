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
            CreateMap<Pizza, PizzaDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Toppings, opt => opt.MapFrom(src => src.PizzaToppings.Select(pt => pt.Topping)));
        }
    }
    public class ToppingProfile : Profile
    {
        public ToppingProfile()
        {
            CreateMap<Topping, ToppingDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<ToppingDTO, Topping>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
