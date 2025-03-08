using DataAccessLayer.Infrastructure.Data;
using DataAccessLayer.Infrastructure.Repositories.RepoInterfaces;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.MenuManagementDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Repositories.RepoImplementations
{
    public class MenuManagementRepo: IMenuManagementRepo
    {
        private readonly RMSServiceDbContext _context;

        public MenuManagementRepo(RMSServiceDbContext context)
        {
            _context = context;
        }

        public async Task AddFoodCategoryAsync(MenuCategoryDetails category)
        {
            await _context.MenuCategories.AddAsync(category);
        }
        public async Task<MenuCategoryDetails?> GetCategoryByIdAsync(Guid categoryId)
        {
            return await _context.MenuCategories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }
        public async Task<MenuCategoryDetails?> GetCategoryByNameAsync(string name)
        {
            // Convert both the field and parameter to lowercase for case-insensitive comparison
            return await _context.MenuCategories
                .Where(c => c.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<MenuCategoryDetails> GetAllFoodItemsByCategoryId(Guid categoryId)
        {
            return await _context.MenuCategories
                .Include(c => c.FoodItems)
                 .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<List<MenuCategoryDetails>> GetFoodCategoriesByIdOrNameAsync(Guid? id, string name)
        {
            var query = _context.MenuCategories.AsQueryable();

            // Filter by id if provided
            if (id.HasValue)
            {
                query = query.Where(fc => fc.CategoryId == id.Value);
            }

            // Filter by name if provided
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(fc => fc.Name.Contains(name));
            }

            return await query.ToListAsync();
        }
        public async Task<List<FoodCategoryResponseDTO>> GetAllCategoriesAndFoodItemsAsync()
        {
            // Fetch all categories and include the related food items for each category
            var categories = await _context.MenuCategories
                .Include(c => c.FoodItems) // Include the food items associated with each category
                .ToListAsync();

            // Manual mapping from MenuCategoryDetails to FoodCategoryResponseDTO
            var categoryDTOs = categories.Select(category => new FoodCategoryResponseDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
                ImagePath = category.ImagePath,
                ImageUrl = category.ImageUrl,

                // Map the associated food items
                FoodItems = category.FoodItems.Select(item => new FoodItemResponseDTO
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    DiscountPercentage = item.DiscountPercentage,
                    ImagePath = item.ImagePath,
                    ImageUrl = item.ImageUrl,
                    OfferPeriod = item.OfferPeriod,
                    OfferDetails = item.OfferDetails,
                    IsSpecialOffer = item.IsSpecialOffer,
                    OrderLink = item.OrderLink,
                    CategoryId = item.CategoryId
                }).ToList()
            }).ToList();

            return categoryDTOs;
        }

        public async Task AddFoodItemAsync(MenuItemDetails foodItem)
        {
            await _context.MenuItems.AddAsync(foodItem);
        }
        public async Task<MenuItemDetails?> GetFoodItemByNameAsync(string name)
        {
            return await _context.MenuItems
                .Where(item => item.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();
        }
        public async Task<MenuItemDetails> GetFoodItemByIdAsync(Guid foodItemId)
        {
            return await _context.MenuItems.FindAsync(foodItemId);
        }
        public async Task<List<FoodItemResponseDTO>> GetAllFoodItemsAsync()
        {
            var foodItems = await _context.MenuItems.ToListAsync();

            // Manual mapping from MenuItemDetails to FoodItemResponseDTO
            var foodItemDTOs = foodItems.Select(item => new FoodItemResponseDTO
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                DiscountPercentage = item.DiscountPercentage, // Assuming this field exists in MenuItemDetails
                ImagePath = item.ImagePath,
                ImageUrl = item.ImageUrl,
                OfferPeriod = item.OfferPeriod, // Assuming this field exists in MenuItemDetails
                OfferDetails = item.OfferDetails, // Assuming this field exists in MenuItemDetails
                IsSpecialOffer = item.IsSpecialOffer, // Assuming this field exists in MenuItemDetails
                OrderLink = item.OrderLink, // Assuming this field exists in MenuItemDetails
                CategoryId = item.CategoryId
            }).ToList();

            return foodItemDTOs;
        }
        public async Task<List<MenuItemDetails>> GetFoodItemsByIdOrNameAsync(Guid? itemId, string name)
        {
            IQueryable<MenuItemDetails> query = _context.MenuItems;

            // Apply filtering based on provided parameters
            if (itemId.HasValue)
            {
                query = query.Where(item => item.ItemId == itemId.Value);
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(item => item.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            // Fetch and return the results
            return await query.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
