using EllipticCurve.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DataModels.OrderManagementModels
{
    public class PaymentOptionDetails
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }

        // Navigation property to relate with Order
        public virtual OrderDetails Order { get; set; }
    }

}
