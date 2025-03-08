﻿using DataAccessLayer.Infrastructure.Repositories.RepoInterfaces;
using DomainLayer.Exceptions;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.MenuManagementDTO;
using DomainLayer.Wrappers.GlobalResponse;
using Serilog;
using ServicesLayer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ServiceImplementations
{
    public class MenuManagementService : IMenuManagementService
    {
        private readonly IMenuManagementRepo _menuManagementRepo;

        public MenuManagementService(IMenuManagementRepo menuManagementRepo)
        {
            _menuManagementRepo = menuManagementRepo;
        }

        public async Task<BaseResponse<Guid>> AddFoodCategoryAsync(FoodCategoryRequestDTO categoryDto, byte[] imageBytes)
        {
            try
            {
                // Check if a category with the same name already exists
                var existingCategory = await _menuManagementRepo.GetCategoryByNameAsync(categoryDto.Name);

                if (existingCategory != null)
                {
                    throw new DuplicateRecordException("Food Categoty name already exists");
                }
                var categoryDetail = new MenuCategoryDetails();
                categoryDetail.CategoryId = Guid.NewGuid();
                categoryDetail.Name = categoryDto.Name;
                categoryDetail.ImageUrl = categoryDto.ImageUrl;
                categoryDetail.ImageData = imageBytes;
                categoryDetail.Description = categoryDto.Description;
                await _menuManagementRepo.AddFoodCategoryAsync(categoryDetail);
                await _menuManagementRepo.SaveChangesAsync();

                return new BaseResponse<Guid>(categoryDetail.CategoryId, HttpStatusCode.OK, true, "Food category added successfully.");
            }
            catch (DuplicateRecordException ex)
            {
                Log.Error("Categoty name already exists");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while adding a food category.");
                throw new CustomInvalidOperationException("An error occurred while fetching special offers.");
            }
        }


        public async Task<MenuCategoryDetails> GetAllFoodItemsWithCategoryId(Guid categoryId)
        {
            // Fetch categories from repository
            var categories = await _menuManagementRepo.GetAllFoodItemsByCategoryId(categoryId);

            // Perform any additional business logic if required
            return categories;
        }

        public async Task<List<FoodCategoryResponseDTO>> GetFoodCategoriesByIdOrNameAsync(Guid? id, string name)
        {
            var categories = await _menuManagementRepo.GetFoodCategoriesByIdOrNameAsync(id, name);

            // Map the entities to DTOs
            var categoryDtos = categories.Select(cat => new FoodCategoryResponseDTO
            {
                CategoryId = cat.CategoryId,
                Name = cat.Name,
                Description = cat.Description,
                ImageData = cat.ImageData,  // Directly map byte array for image data
                FoodItems = cat.FoodItems
                    .Select(item => new FoodItemResponseDTO
                    {
                        ItemId = item.ItemId,
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        DiscountPercentage = item.DiscountPercentage,
                        ImageData = item.ImageData,
                        // Add other properties as needed
                    }).ToList()
            }).ToList();

            return categoryDtos;
        }
        public async Task<List<FoodCategoryResponseDTO>> GetAllCategoriesAndFoodItemsAsync()
        {
            var categories = await _menuManagementRepo.GetAllCategoriesAndFoodItemsAsync();

            // Manual mapping from MenuCategoryDetails to FoodCategoryResponseDTO
            //var categoryDTOs = categories.Select(category => new FoodCategoryResponseDTO
            //{
            //    CategoryId = category.CategoryId,
            //    Name = category.Name,
            //    Description = category.Description,
            //    ImageData = category.ImageData,

            //    // Map associated food items
            //    FoodItems = category.FoodItems.Select(item => new FoodItemResponseDTO
            //    {
            //        ItemId = item.ItemId,
            //        Name = item.Name,
            //        Description = item.Description,
            //        Price = item.Price,
            //        DiscountPercentage = item.DiscountPercentage,
            //        ImageData = item.ImageData,
            //        OfferPeriod = item.OfferPeriod,
            //        OfferDetails = item.OfferDetails,
            //        IsSpecialOffer = item.IsSpecialOffer,
            //        OrderLink = item.OrderLink,
            //        CategoryId = item.CategoryId
            //    }).ToList()
            //}).ToList();

            return categories;
        }

        public async Task<BaseResponse<Guid>> AddFoodItemAsync(FoodItemRequestDTO foodItemDto, Guid categoryId, byte[] imageBytes)
        {
            try
            {
                // Check if the category exists
                var existingCategory = await _menuManagementRepo.GetCategoryByIdAsync(categoryId);
                if (existingCategory == null)
                {
                    throw new DuplicateRecordException("The specified category does not exist.");
                }
                // Check if a food item with the same name already exists
                var existingFoodItem = await _menuManagementRepo.GetFoodItemByNameAsync(foodItemDto.Name);
                if (existingFoodItem != null)
                {
                    throw new DuplicateRecordException("Food Item name already exists");
                }
                var foodItemDetail = new MenuItemDetails();
                foodItemDetail.ItemId = Guid.NewGuid();
                foodItemDetail.Name = foodItemDto.Name;
                foodItemDetail.Description = foodItemDto.Description;
                foodItemDetail.Price = foodItemDto.Price;
                foodItemDetail.ImageUrl = foodItemDto.ImageUrl;
                foodItemDetail.ImageData = imageBytes;
                foodItemDetail.IsSpecialOffer = foodItemDto.IsSpecialOffer;
                foodItemDetail.CategoryId = categoryId;
                await _menuManagementRepo.AddFoodItemAsync(foodItemDetail);
                await _menuManagementRepo.SaveChangesAsync();

                return new BaseResponse<Guid>(foodItemDetail.ItemId, HttpStatusCode.OK, true, "Food item added successfully.");
            }
            catch (DuplicateRecordException ex)
            {
                Log.Error("Food Item name already exists");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while adding a food item.");
                throw new CustomInvalidOperationException("An error occurred while adding a food item.");
            }
        }

        public async Task<List<FoodItemResponseDTO>> GetAllFoodItemsAsync()
        {
            // Fetch all food items from repository
            var foodItems = await _menuManagementRepo.GetAllFoodItemsAsync();

            // Perform any mapping or additional logic if necessary
            return foodItems;
        }

        public async Task<List<FoodItemResponseDTO>> GetFoodItemsByIdOrNameAsync(Guid? itemId, string name)
        {
            // Fetch the filtered food items from the repository
            var items = await _menuManagementRepo.GetFoodItemsByIdOrNameAsync(itemId, name);

            // Map to DTOs
            var itemDtos = items.Select(item => new FoodItemResponseDTO
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                DiscountPercentage = item.DiscountPercentage,
                ImageData = item.ImageData,  // Assuming ImageData is stored as a byte array
                OfferPeriod = item.OfferPeriod,
                OfferDetails = item.OfferDetails,
                IsSpecialOffer = item.IsSpecialOffer,
                OrderLink = item.OrderLink,
                CategoryId = item.CategoryId
            }).ToList();

            return itemDtos;
        }

    }

}
