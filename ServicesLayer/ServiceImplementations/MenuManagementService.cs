using DataAccessLayer.Infrastructure.Repositories.RepoInterfaces;
using DomainLayer.Exceptions;
using DomainLayer.Models.DataModels.HomepageManagementModels;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.HomepageManagementDTO;
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

        public async Task<BaseResponse<Guid>> AddFoodCategoryAsync(FoodCategoryRequestDTO categoryDto)
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
                categoryDetail.ImagePath = categoryDto.ImagePath;
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
                Log.Error($"An error occurred while adding a food category. {ex}");
                throw new CustomInvalidOperationException($"An error occurred while adding a food category. {ex}");
            }
        }

        public async Task<FoodCategoryResponseDTO?> GetAllFoodItemsWithCategoryId(Guid categoryId)
        {
            // Fetch category from repository
            var category = await _menuManagementRepo.GetAllFoodItemsByCategoryId(categoryId);

            if (category == null)
            {
                return null; // Return null if no category is found
            }

            return new FoodCategoryResponseDTO
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description,
                ImagePath = category.ImagePath,

                // Map associated food items
                FoodItems = category.FoodItems?.Any() == true
                    ? category.FoodItems.Select(item => new FoodItemResponseDTO
                    {
                        ItemId = item.ItemId,
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        DiscountPercentage = item.DiscountPercentage,
                        ImagePath = item.ImagePath,
                        ImageUrl = item.ImageUrl,
                        OrderLink = item.OrderLink,
                        CategoryId = item.CategoryId,

                        // ✅ Fetch and Map Special Groups
                        SpecialGroups = item.FoodItemSpecialGroups?
                    .Where(fsg => fsg.HomepageSpecialGroup != null) // Ensure valid groups
                    .Select(fsg => new HomepageSpecialGroupResponseDTO
                    {
                        Id = fsg.HomepageSpecialGroup.GroupId,
                        GroupName = fsg.HomepageSpecialGroup.GroupName,
                        GroupDescription = fsg.HomepageSpecialGroup.GroupDescription,
                    }).ToList()
                    }).ToList()
                    : new List<FoodItemResponseDTO>() // Empty list if no items
            };
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
                ImagePath = cat.ImagePath,  
                FoodItems = cat.FoodItems
                    .Select(item => new FoodItemResponseDTO
                    {
                        ItemId = item.ItemId,
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        DiscountPercentage = item.DiscountPercentage,
                        ImagePath = item.ImagePath,
                    
                    }).ToList()
            }).ToList();

            return categoryDtos;
        }
        public async Task<List<FoodCategoryandItemOnlyResponseDTO>> GetAllCategoriesAndFoodItemsAsync()
        {
            var categories = await _menuManagementRepo.GetAllCategoriesAndFoodItemsAsync();

            return categories;
        }

        public async Task<BaseResponse<Guid>> AddFoodItemAsync(FoodItemRequestDTO foodItemDto, Guid categoryId)
        {
            try
            {
                // ✅ Check if category exists
                var existingCategory = await _menuManagementRepo.GetCategoryByIdAsync(categoryId);
                if (existingCategory == null)
                {
                    throw new DuplicateRecordException("The specified category does not exist.");
                }

                // ✅ Check if the food item name already exists
                var existingFoodItem = await _menuManagementRepo.GetFoodItemByNameAsync(foodItemDto.Name);
                if (existingFoodItem != null)
                {
                    throw new DuplicateRecordException("Food Item name already exists.");
                }


                // ✅ Check if SpecialGroupIds are provided
                if (foodItemDto.SpecialGroupIds != null && foodItemDto.SpecialGroupIds.Any())
                {
                    // ✅ Fetch all matching special groups in a single query
                    var existingSpecialGroups = await _menuManagementRepo
                        .GetSpecialGroupsByIdsAsync(foodItemDto.SpecialGroupIds);

                    // ✅ Find missing IDs that don’t exist in the database
                    var missingIds = foodItemDto.SpecialGroupIds
                        .Except(existingSpecialGroups.Select(sg => sg.GroupId))
                        .ToList();

                    // ✅ If any ID is missing, throw an error
                    if (missingIds.Any())
                    {
                        throw new NotFoundException($"The following SpecialGroupIds do not exist: {string.Join(", ", missingIds)}");
                    }
                }



                // ✅ Create the new Food Item
                var foodItemDetail = new MenuItemDetails
                {
                    ItemId = Guid.NewGuid(),
                    Name = foodItemDto.Name,
                    Description = foodItemDto.Description,
                    Price = foodItemDto.Price,
                    ImageUrl = foodItemDto.ImageUrl,
                    CategoryId = categoryId,
                    FoodItemSpecialGroups = new List<FoodItemSpecialGroupMap>()
                };

                //// ✅ Assign Special Groups if provided
                //if (foodItemDto.SpecialGroupIds != null && foodItemDto.SpecialGroupIds.Any())
                //{
                //    foreach (var specialGroupId in foodItemDto.SpecialGroupIds)
                //    {
                //        var specialGroup = await _menuManagementRepo.GetSpecialGroupByIdAsync(specialGroupId);
                //        if (specialGroup != null)
                //        {
                //            foodItemDetail.FoodItemSpecialGroups.Add(new FoodItemSpecialGroupMap
                //            {
                //                FoodItemId = foodItemDetail.ItemId,
                //                SpecialGroupId = specialGroup.GroupId
                //            });
                //        }
                //    }
                //}

                // ✅ Assign Special Groups if provided
                if (foodItemDto.SpecialGroupIds != null && foodItemDto.SpecialGroupIds.Any())
                {
                    // ✅ Fetch all matching special groups in a single query
                    var existingSpecialGroups = await _menuManagementRepo
                        .GetSpecialGroupsByIdsAsync(foodItemDto.SpecialGroupIds);

                    // ✅ Ensure the FoodItemSpecialGroups list is initialized
                    foodItemDetail.FoodItemSpecialGroups ??= new List<FoodItemSpecialGroupMap>();

                    // ✅ Add only valid special groups
                    foreach (var specialGroup in existingSpecialGroups)
                    {
                        foodItemDetail.FoodItemSpecialGroups.Add(new FoodItemSpecialGroupMap
                        {
                            FoodItemId = foodItemDetail.ItemId,
                            SpecialGroupId = specialGroup.GroupId
                        });
                    }
                }


                // ✅ Save the food item to the DB
                await _menuManagementRepo.AddFoodItemAsync(foodItemDetail);

                // ✅ Save the changes to the database
                await _menuManagementRepo.SaveChangesAsync();

                return new BaseResponse<Guid>(foodItemDetail.ItemId, HttpStatusCode.Created, true, "Food item added successfully.");
            }
            catch (DuplicateRecordException ex)
            {
                Log.Error("Food Item name already exists.");
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

        public async Task<List<FoodCategoryResponseDTO>> GetAllFoodCategoriesOnlyAsync()
        {
            // Fetch all food items from repository
            var foodItems = await _menuManagementRepo.GetAllFoodCategoriesOnlyAsync();

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
                ImagePath = item.ImagePath,  
                OrderLink = item.OrderLink,
                CategoryId = item.CategoryId
            }).ToList();

            return itemDtos;
        }

    }

}
