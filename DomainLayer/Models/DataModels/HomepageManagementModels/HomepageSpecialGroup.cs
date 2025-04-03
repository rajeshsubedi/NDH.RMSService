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
    

        // ✅ Add Many-to-Many Relationship
        public List<FoodItemSpecialGroupMap> FoodItemSpecialGroups { get; set; }
    }

}
