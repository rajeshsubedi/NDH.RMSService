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
        public Guid UserId { get; set; } // Foreign key to UserRegistrationDetails
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // List of items in the order (One-to-many relationship)
        public List<OrderedItemsDetails>? OrderedItems { get; set; }

        // Indicates if the delivery should be made as soon as possible
        public bool IsAsSoonAsPossible { get; set; }

        // Optional: Specific delivery date and time
        public DateTime? DeliveryDateTime { get; set; }

        // Order status (e.g., Pending, Completed, Cancelled)
        public string OrderStatus { get; set; }

        // One-to-one relationship: Each order has one delivery address
        public DeliveryAddressDetails DeliveryAddress { get; set; } // Navigation property

        // One-to-one relationship: Each order has one payment option
        public PaymentOptionDetails PaymentOption { get; set; } // Navigation property

        // Navigation property to UserRegistrationDetails
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
