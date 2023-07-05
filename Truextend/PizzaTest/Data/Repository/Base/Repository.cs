using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Exceptions;
using Truextend.PizzaTest.Data.Models.Base;

namespace Truextend.PizzaTest.Data.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly PizzaDbContext dbContext;
        public Repository(PizzaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            List<T> values = await dbContext.Set<T>().ToListAsync();
            return values;
        }
        public async Task<T> GetByIdAsync(Guid id)
        {
            T value = await dbContext.Set<T>().FindAsync(id);
            if (value == null)
            {
                throw new DatabaseException($"Entity of type {typeof(T).Name} with id {id} not found.");
            }
            return value;
        }
        public async Task<T> CreateAsync(T entity)
        {
            try
            {
                dbContext.Set<T>().Add(entity);
                await dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception e)
            {
                throw new DatabaseException("ERROR: " + e.InnerException.Message);
            }
        }
        public async Task<T> DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return entity;
        }
        
    }
}
