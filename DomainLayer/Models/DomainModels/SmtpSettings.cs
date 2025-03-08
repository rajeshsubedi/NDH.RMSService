using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DomainModels
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string CompanyName { get; set; } // Add this line
        public string EmailVerificationApiUrl { get; set; }
        public string EmailVerificationApiKey {  get; set; }
    }
}
