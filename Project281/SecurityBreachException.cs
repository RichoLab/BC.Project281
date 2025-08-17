using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project281
{
    internal class SecurityBreachException: Exception
    {
        public SecurityBreachException() { }
        public SecurityBreachException(string message) : base(message) { }
        public SecurityBreachException(string message, Exception innerException) : base(message, innerException) { }
    }
}
