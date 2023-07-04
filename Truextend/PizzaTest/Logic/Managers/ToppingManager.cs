using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Core.Exceptions;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Data;
using Truextend.PizzaTest.Logic.Managers.Base;

namespace Truextend.PizzaTest.Logic.Managers
{
    public class ToppingManager : IGenericManager<Topping>
    {
        private readonly IUnitOfWork _uow;

        public ToppingManager(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<IEnumerable<Topping>> GetAllAsync()
        {
            return await _uow.ToppingRepository.GetAllAsync();
        }
        public async Task<Topping> GetByIdAsync(Guid id)
        {
            return await _uow.ToppingRepository.GetByIdAsync(id);
        }
        public async Task<Topping> CreateAsync(Topping topping)
        {
            return await _uow.ToppingRepository.CreateAsync(topping);
        }
        public async Task<Topping> UpdateAsync(Guid id, Topping toppingToUpdate)
        {
            Topping topping = await _uow.ToppingRepository.GetByIdAsync(id);
            if (topping != null)
            {
                topping.Name = toppingToUpdate.Name;

            }
            return await _uow.ToppingRepository.UpdateAsync(toppingToUpdate);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            Topping toppingFound = await _uow.ToppingRepository.GetByIdAsync(id);
            await _uow.ToppingRepository.DeleteAsync(toppingFound);
            return await _uow.ToppingRepository.GetByIdAsync(id) == null;
        }
    }
}
