using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Models.DataModels.OrderManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Wrappers.DTO.OrderManagementDTO
{
    public class OrderDetailsResponseDTO
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderedItemDetailsResponseDTO> OrderedItems { get; set; }
    }

    public class OrderedItemDetailsResponseDTO
    {
        public Guid FoodItemId { get; set; } // Foreign key to FoodItemDetails
        public String ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } // Calculated as Quantity * UnitPrice
        public MenuItemDetails FoodItem { get; set; } // Navigation property to FoodItemDetails
    }
}
