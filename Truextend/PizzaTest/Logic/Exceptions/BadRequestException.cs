using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models.Validation;

namespace Truextend.PizzaTest.Logic.Exceptions
{
    public class BadRequestException : Exception
    {
        public readonly IDictionary<string, List<string>> Details;
        public BadRequestException(string message) : base(message) { }

        public BadRequestException(string message, Exception innerException) : base(string.Format(message), innerException) { }

        public BadRequestException(string message, IEnumerable<ValidationError> errors) : base(message)
        {
            Details = new Dictionary<string, List<string>>();
            foreach (ValidationError error in errors)
            {
                List<string> messages;
                if (Details.ContainsKey(error.Field))
                {
                    messages = Details[error.Field];
                }
                else
                {
                    messages = new List<string>();
                    Details[error.Field] = messages;
                }
                messages.Add(error.Message);
            }
        }
    }
}
