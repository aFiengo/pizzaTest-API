using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truextend.PizzaTest.Logic.Exceptions
{
    public class LogicException : Exception
    {
        public LogicException(string message) : base(message) { }

        public LogicException(string message, Exception innerException) : base(string.Format(message), innerException) { }
    }
}
