using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DataModels.MenuManagementModels
{
    public class MenuItemDetails
    {

        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; } 
        public decimal? DiscountPercentage { get; set; }
        public string? ImageUrl { get; set; }

        public string? ImagePath { get; set; }  
        public string? OfferPeriod { get; set; }
        public string? OfferDetails { get; set; }
        public bool? IsSpecialOffer { get; set; } 
        public string? OrderLink { get; set; }
        public Guid CategoryId { get; set; }
        public MenuCategoryDetails Category { get; set; }
    }
}
