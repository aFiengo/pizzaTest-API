using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Truextend.PizzaTest.Data.Models.Validation
{
    public class IValidatable
    {
        public interface IValidatable
        {
            bool IsValid();

            IEnumerable<ValidationError> GetErrors();
        }
    }
}
