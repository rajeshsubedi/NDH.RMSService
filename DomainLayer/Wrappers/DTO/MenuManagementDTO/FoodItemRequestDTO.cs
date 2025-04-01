using DomainLayer.Models.DataModels.HomepageManagementModels;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.HomepageManagementDTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Wrappers.DTO.MenuManagementDTO
{
    public class FoodItemRequestDTO
    {
      
        public string Name { get; set; }
    
        public string Description { get; set; }

        public decimal? Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImagePath { get; set; }  

        public Guid CategoryId { get; set; }

        public List<Guid>? SpecialGroupIds { get; set; }

    }

    public class FoodItemResponseDTO
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string? ImagePath { get; set; } 
        public string? ImageUrl { get; set; }
        public string? OrderLink { get; set; }
        public Guid CategoryId { get; set; }

        // ✅ Include Special Groups
        public List<HomepageSpecialGroupResponseDTO>? SpecialGroups { get; set; }

    }


}
