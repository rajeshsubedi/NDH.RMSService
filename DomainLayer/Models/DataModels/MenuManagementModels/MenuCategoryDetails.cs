using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DataModels.MenuManagementModels
{
    public class MenuCategoryDetails
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }

        public byte[]? ImageData { get; set; }  // Store the image as a byte array.

        // Navigation property to the related FoodItemsDetails (One-to-Many)
        public ICollection<MenuItemDetails> FoodItems { get; set; } = new List<MenuItemDetails>();
    }

}
