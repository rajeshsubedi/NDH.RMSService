using DomainLayer.Models.DataModels.AuthenticationModels;
using DomainLayer.Models.DataModels.MenuManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DataModels.OrderManagementModels
{
    public class OrderDetails
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; } 
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderedItemsDetails>? OrderedItems { get; set; }
        public bool IsAsSoonAsPossible { get; set; }

        public DateTime? DeliveryDateTime { get; set; }
        public string OrderStatus { get; set; }

        public DeliveryAddressDetails DeliveryAddress { get; set; } 

        public PaymentOptionDetails PaymentOption { get; set; }

        public UserRegistrationDetails User { get; set; }
    }



    public class OrderedItemsDetails
    {
        public Guid OrderItemId { get; set; }
        public String ItemName { get; set; }
        public Guid OrderId { get; set; } // Foreign key to OrderDetails
        public Guid FoodItemId { get; set; } // Foreign key to FoodItemDetails
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } // Calculated as Quantity * UnitPrice
        public OrderDetails? Order { get; set; } // Navigation property to OrderDetails
        public MenuItemDetails? FoodItem { get; set; } // Navigation property to FoodItemDetails
    }
}
