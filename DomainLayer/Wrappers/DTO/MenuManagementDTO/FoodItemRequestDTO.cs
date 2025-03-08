using DomainLayer.Models.DataModels.MenuManagementModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Wrappers.DTO.MenuManagementDTO
{
    public class FoodItemRequestDTO
    {
      
        public string Name { get; set; }
    
        public string Description { get; set; }

        public decimal? Price { get; set; }

        //public decimal? DiscountPercentage { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? ImageData { get; set; }  // Store the image as a byte array.

        //public string? OfferPeriod { get; set; }

        //public string? OfferDetails { get; set; }

        public bool? IsSpecialOffer { get; set; }
        public Guid CategoryId { get; set; }

    //public string? OrderLink { get; set; }

    }

    public class FoodItemResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public byte[]? ImageData { get; set; }  // Store the image as a byte array.

        public string? ImageUrl { get; set; }
        public string? OfferPeriod { get; set; }
        public string? OfferDetails { get; set; }
        public bool? IsSpecialOffer { get; set; } // Flag to indicate special offer
        public string? OrderLink { get; set; }
        public Guid CategoryId { get; set; }

    }


}
