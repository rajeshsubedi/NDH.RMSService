using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Wrappers.DTO.OrderManagementDTO
{
    // Delivery address details
    public class DeliveryAddressRequestDTO
    {
        public string Street { get; set; } // Street address
        public string City { get; set; } // City
        public string State { get; set; } // State  
        public string PostalCode { get; set; } // Postal code
        public string Country { get; set; } // Country
    }
}
