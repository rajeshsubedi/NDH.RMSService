using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Wrappers.DTO.AuthenticationDTO
{
    public class LoginRequestDTO
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }
    }
}
