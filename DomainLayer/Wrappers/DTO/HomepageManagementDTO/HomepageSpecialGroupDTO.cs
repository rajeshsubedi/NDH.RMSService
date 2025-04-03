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
    }

    public class HomepageSpecialGroupResponseDTO
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }

    }
}
