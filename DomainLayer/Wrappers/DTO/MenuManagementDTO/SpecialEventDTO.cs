using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Wrappers.DTO.MenuManagementDTO
{
    public class SpecialEventDTO
    {
        public string EventName { get; set; }

        //public DateTime EventDate { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImagePath { get; set; }  
    }
}
