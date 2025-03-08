using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class CustomInvalidOperationException : Exception
    {
        public CustomInvalidOperationException() : base() { }
        public CustomInvalidOperationException(string message) : base (message) { }
    }
}
