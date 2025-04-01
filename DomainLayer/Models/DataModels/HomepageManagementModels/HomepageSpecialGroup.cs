using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models.DataModels.MenuManagementModels;

namespace DomainLayer.Models.DataModels.HomepageManagementModels
{
    public class HomepageSpecialGroup
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string GroupType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public bool IsDiscounted { get; set; }
        public string DiscountedRate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? ImageUrl { get; set; }

        // ✅ Add Many-to-Many Relationship
        public List<FoodItemSpecialGroupMap> FoodItemSpecialGroups { get; set; }
    }

}
