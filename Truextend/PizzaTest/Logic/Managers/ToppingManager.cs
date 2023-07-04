using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Data;
using Truextend.PizzaTest.Logic.Managers.Base;
using Truextend.PizzaTest.Logic.Managers.Interface;
using AutoMapper;
using Truextend.PizzaTest.Logic.Models;
using Truextend.PizzaTest.Core.Exceptions;

namespace Truextend.PizzaTest.Logic.Managers
{
    public class ToppingManager : IToppingManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ToppingManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ToppingDTO>> GetAllAsync()
        {
            var toppings = await _uow.PizzaRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ToppingDTO>>(toppings);
        }
        public async Task<ToppingDTO> GetByIdAsync(Guid id)
        {
            var topping =  await _uow.ToppingRepository.GetByIdAsync(id);
            return _mapper.Map<ToppingDTO>(topping);
        }
        public async Task<ToppingDTO> CreateAsync(ToppingDTO toppingToAdd)
        {
            var topping = _mapper.Map<Topping>(toppingToAdd);
            var createdTopping = await _uow.ToppingRepository.CreateAsync(topping);
            return _mapper.Map<ToppingDTO>(createdTopping);
        }
        public async Task<ToppingDTO> UpdateAsync(Guid id, ToppingDTO toppingToUpdate)
        {
            var existingTopping = await _uow.ToppingRepository.GetByIdAsync(id);
            if (existingTopping == null)
            {
                throw new NotFoundException($"Topping with ID {id} not found.");
            }

            var updatedTopping = _mapper.Map<Topping>(toppingToUpdate);
            existingTopping.Name = updatedTopping.Name;

            var result = await _uow.ToppingRepository.UpdateAsync(existingTopping);
            return _mapper.Map<ToppingDTO>(result);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingTopping = await _uow.ToppingRepository.GetByIdAsync(id);
            if (existingTopping == null)
            {
                throw new NotFoundException($"Topping with ID {id} not found.");
            }

            await _uow.ToppingRepository.DeleteAsync(existingTopping);
            return await _uow.ToppingRepository.GetByIdAsync(id) == null;
        }
    }
}
