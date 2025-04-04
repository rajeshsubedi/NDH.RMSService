using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DataModels.HomepageManagementModels
{
    public class CompanyDetails
    {
        public Guid CompanyId { get; set; } = Guid.NewGuid(); // Auto-generate ID
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Website { get; set; }
    }

}
