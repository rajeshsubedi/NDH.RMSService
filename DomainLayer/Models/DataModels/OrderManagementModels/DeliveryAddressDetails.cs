using EllipticCurve.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DataModels.OrderManagementModels
{
    public class DeliveryAddressDetails
    {
        public Guid AddressId { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        // Indicates if this address is the default one for the user
        public bool IsDefault { get; set; }

        // Navigation property to relate with Order
        public virtual OrderDetails Order { get; set; }
    }
}
