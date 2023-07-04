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
        ISizeRepository SizeRepository { get; }
        IToppingRepository ToppingRepository { get; }
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
        void Save();
    }
}
