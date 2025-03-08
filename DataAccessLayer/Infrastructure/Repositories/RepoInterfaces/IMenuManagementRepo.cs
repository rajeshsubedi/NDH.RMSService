﻿using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.MenuManagementDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Repositories.RepoInterfaces
{
    public interface IMenuManagementRepo
    {
        Task AddFoodCategoryAsync(MenuCategoryDetails category);
        Task<MenuCategoryDetails> GetCategoryByIdAsync(Guid categoryId);
        Task<MenuCategoryDetails?> GetCategoryByNameAsync(string name);
        Task<MenuCategoryDetails> GetAllFoodItemsByCategoryId(Guid categoryId);
        Task<List<MenuCategoryDetails>> GetFoodCategoriesByIdOrNameAsync(Guid? id, string name);
        Task<List<FoodCategoryResponseDTO>> GetAllCategoriesAndFoodItemsAsync();
        Task AddFoodItemAsync(MenuItemDetails foodItem);
        Task<MenuItemDetails?> GetFoodItemByNameAsync(string name);
        Task<MenuItemDetails> GetFoodItemByIdAsync(Guid foodItemId);
        Task<List<FoodItemResponseDTO>> GetAllFoodItemsAsync();
        Task<List<MenuItemDetails>> GetFoodItemsByIdOrNameAsync(Guid? itemId, string name);
        Task SaveChangesAsync();
    }
}
