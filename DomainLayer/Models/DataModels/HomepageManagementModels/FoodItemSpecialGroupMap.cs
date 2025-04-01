using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models.DataModels.MenuManagementModels;

namespace DomainLayer.Models.DataModels.HomepageManagementModels
{
    public class FoodItemSpecialGroupMap
    {
        public Guid FoodItemId { get; set; }
        public MenuItemDetails FoodItem { get; set; }

        public Guid SpecialGroupId { get; set; }
        public HomepageSpecialGroup HomepageSpecialGroup { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedBy { get; set; } // User ID (Optional)
    }
}
