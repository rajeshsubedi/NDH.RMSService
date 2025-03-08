using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class UserUnauthenticatedException : Exception
    {
        public UserUnauthenticatedException(string message) : base(message) { }
    }
}
