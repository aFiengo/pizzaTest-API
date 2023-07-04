using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truextend.PizzaTest.Logic.Exceptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException(string message) : base(message) { }

        public AlreadyExistException(string message, Exception innerException) : base(string.Format(message), innerException) { }
    }
}
