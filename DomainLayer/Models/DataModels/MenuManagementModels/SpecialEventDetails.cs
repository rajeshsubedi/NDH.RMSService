using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models.DataModels.MenuManagementModels
{
    public class SpecialEventDetails
    {
        public Guid EventId { get; set; } // Primary Key

        public string EventName { get; set; }

        public DateTime EventDate { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImagePath { get; set; }  
    }
}
