using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Exceptions;
using Truextend.PizzaTest.Data.Repository;
using Truextend.PizzaTest.Data.Repository.Interfaces;

namespace Truextend.PizzaTest.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PizzaDbContext _pizzaDbContext;
        private readonly IPizzaRepository _pizza;
        private readonly IToppingRepository _topping;

        public UnitOfWork(PizzaDbContext dbContext)
        {
            _pizzaDbContext = dbContext;
            _pizza = new PizzaRepository(_pizzaDbContext);
            _topping = new ToppingRepository(_pizzaDbContext);
        }
        public void BeginTransaction()
        {
            _pizzaDbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _pizzaDbContext.Database.CommitTransaction();
        }

        public void RollBackTransaction()
        {
            _pizzaDbContext.Database.RollbackTransaction();
        }

        public void Save()
        {
            try
            {
                BeginTransaction();
                _pizzaDbContext.SaveChanges();
                CommitTransaction();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                RollBackTransaction();
                string message = $"Error to save changes on Database -> Save() {Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                throw new DatabaseException("Can not save changes, error in Database", ex.InnerException);
            }
            catch (DbUpdateException ex)
            {
                RollBackTransaction();
                string message = $"Error to save changes on Database -> Save() {Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                throw new DatabaseException("Can not save changes, error in Database", ex.InnerException);
            }
            catch (Exception ex)
            {
                string message = $"Error to save changes on Database -> Save() {Environment.NewLine}Message: {ex.Message}{Environment.NewLine}";
                throw new DatabaseException("Can not save changes, error in Database", ex.InnerException);
            }
        }
        public IPizzaRepository PizzaRepository
        {
            get { return _pizza;  }
        }
        public IToppingRepository ToppingRepository
        {
            get { return _topping; }
        }
    }
}
