using DomainLayer.Models.DataModels.OrderManagementModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DomainLayer.Wrappers.DTO.OrderManagementDTO
{
    public class PlaceOrderRequestDTO
    {
        [Required]
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; } // User placing the order

        [Required]
        [MinLength(1, ErrorMessage = "At least one food item must be ordered.")]
        [JsonPropertyName("orderedFoodItems")]
        public List<PlaceItemsRequestDTO> OrderedFoodItems { get; set; } // List of food item IDs with quantities

        [Required]
        [JsonPropertyName("deliveryAddress")]
        public DeliveryAddressRequestDTO DeliveryAddress { get; set; } // Delivery address details

        [JsonPropertyName("isAsSoonAsPossible")]
        public bool IsAsSoonAsPossible { get; set; } // Indicates if the delivery should be made as soon as possible

        [JsonPropertyName("deliveryDateTime")]
        public DateTime? DeliveryDateTime { get; set; } // Optional: Specific delivery date and time

        [Required]
        [JsonPropertyName("paymentMethod")]
        public PaymentMethod PaymentMethod { get; set; } // Payment method

        [Required]
        [JsonPropertyName("orderStatus")]
        public OrderStatus OrderStatus { get; set; } // Status of the order

        [Required]
        [JsonPropertyName("paymentStatus")]
        public PaymentStatus PaymentStatus { get; set; } // Status of the payment
    }

    public class PlaceItemsRequestDTO
    {
        public Guid FoodItemId { get; set; } // ID of the food item
        public int Quantity { get; set; } // Quantity of the food item
    }

    public enum OrderStatus
    {
        Pending,                 //0
        Preparing,               //1
        OutForDelivery,          //2
        Delivered,               //3
        Canceled                 //4
    }

    public enum PaymentMethod
    {
        CashonDelivery,            //0
        IMEPay,                    //1
        eSewa,                     //2 
        BankTransfer,              //3
        KhaltiDigitalWallet,       //4
    }

    public enum PaymentStatus
    {
        Pending,                 //0
        Preparing,               //1
        OutForDelivery,          //2
        Delivered,               //3
        Canceled
    }
}
