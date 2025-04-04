using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DataModels.HomepageManagementModels
{
    public class BannerDetails
    {
        public Guid BannerId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
