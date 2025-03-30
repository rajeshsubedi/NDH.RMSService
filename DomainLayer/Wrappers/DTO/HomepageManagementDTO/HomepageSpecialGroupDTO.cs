using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Wrappers.DTO.HomepageManagementDTO
{
    public class HomepageSpecialGroupDTO
    {
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string GroupType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public string ImageUrl { get; set; }
    }
}
