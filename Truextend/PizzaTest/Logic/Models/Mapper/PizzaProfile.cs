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
                .ForMember(dest => dest.Toppings, opt => opt.MapFrom(src => src.Toppings))
                .ReverseMap();
            this.CreateMap<Pizza, PizzaNameDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            this.CreateMap<PizzaCreateDTO, Pizza>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            this.CreateMap<Topping, ToppingDTO>().ReverseMap();
        }
    }
   
}
