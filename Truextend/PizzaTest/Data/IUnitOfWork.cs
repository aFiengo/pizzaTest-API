using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Repository.Interfaces;

namespace Truextend.PizzaTest.Data
{
    public interface IUnitOfWork
    {
        IPizzaRepository PizzaRepository { get; }
        IToppingRepository ToppingRepository { get; }
        Task<int> SaveChangesAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
        void Save();
    }
}
