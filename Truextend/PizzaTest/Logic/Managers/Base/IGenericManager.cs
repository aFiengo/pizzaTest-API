using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truextend.PizzaTest.Logic.Managers.Base
{
    public interface IGenericManager<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T item);
        Task<T> UpdateAsync(Guid id, T item);
        Task<bool> DeleteAsync(Guid itemId);
    }
}
