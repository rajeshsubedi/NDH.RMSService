﻿using DomainLayer.Models.DataModels.MenuManagementModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Wrappers.DTO.MenuManagementDTO
{
    public class FoodCategoryRequestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageData { get; set; }  // Store the image as a byte array.
    }

    public class FoodCategoryResponseDTO
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[]? ImageData { get; set; }  // Store the image as a byte array.
        public string? ImageUrl { get; set; }
        public List<FoodItemResponseDTO>? FoodItems { get; set; }

    }
}
