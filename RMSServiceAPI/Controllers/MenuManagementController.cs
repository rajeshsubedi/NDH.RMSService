using DomainLayer.Exceptions;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.MenuManagementDTO;
using DomainLayer.Wrappers.GlobalResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ServicesLayer.ServiceInterfaces;
using System.Net;

namespace RMSServiceAPI.Controllers
{
    [ApiController]
    [Route("api/menu")]
    public class MenuManagementController : ControllerBase
    {
        private readonly IMenuManagementService _menuManagementService;

        public MenuManagementController(IMenuManagementService menuManagementService)
        {
            _menuManagementService = menuManagementService;
        }

        [HttpPost("add-food-category")]
        public async Task<BaseResponse<Guid>> AddFoodCategory([FromForm] FoodCategoryRequestDTO categoryDto)
        {
            try
            {
        
                // Add the food category
                var response = await _menuManagementService.AddFoodCategoryAsync(categoryDto);

                // Return the success response
                return new BaseResponse<Guid>(
                    response._data,
                    HttpStatusCode.Created,
                    true,
                    "Food category added successfully."
                );
            }
            catch (DuplicateRecordException ex)
            {
                Log.Error("Categoty name already exists");
                throw;
            }
            catch (CustomInvalidOperationException ex)
            {
                Log.Error("Add Categories failed");
                throw; // This will be caught by the middleware
            }
            catch (Exception ex)
            {
                throw new CustomInvalidOperationException("An error occurred while adding categories.");
            }
        }

        // GET: api/food-category/all
        [HttpGet("get-fooditems")]
        public async Task<BaseResponse<object>> GetFoodItems([FromQuery] Guid? categoryId)
        {
            try
            {
                object result;

                if (categoryId.HasValue)
                {
                    // Fetch food items by category ID
                    var categories = await _menuManagementService.GetAllFoodItemsWithCategoryId(categoryId.Value);

                    if (categories == null)
                    {
                        throw new NotFoundException("No food categories found.");
                    }
                    if (!categories.Any() || categories.All(c => c.FoodItems.Count == 0))
                    {
                        throw new NotFoundException("No food items found.");
                    }

                    result = categories;
                }
                else
                {
                    // Fetch all food items
                    var allItems = await _menuManagementService.GetAllFoodItemsAsync();

                    if (allItems == null || !allItems.Any())
                    {
                        throw new NotFoundException();
                    }

                    result = allItems;
                }

                // Return success response
                return new BaseResponse<object>(result, HttpStatusCode.OK, true, "Food items retrieved successfully.");
            }
            catch (NotFoundException ex)
            {
                Log.Error("An error occurred while retrieving food items.", ex);
                throw new NotFoundException("No food items found.");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while retrieving food items.", ex);
                throw new CustomInvalidOperationException("An error occurred while retrieving food items.");
            }
        }


        [HttpGet("filter-categories")]
        public async Task<BaseResponse<List<FoodCategoryResponseDTO>>> GetFoodCategoriesByIdOrName([FromQuery] Guid? categoryId, [FromQuery] string name)
        {
            try
            {
                // Fetch the filtered categories from the service
                var categories = await _menuManagementService.GetFoodCategoriesByIdOrNameAsync(categoryId, name);

                // Check if categories are empty
                if (categories == null || categories.Count == 0)
                {
                    throw new NotFoundException("No food categories found matching the filter criteria."
                    );
                }

                // Return the success response with filtered categories
                return new BaseResponse<List<FoodCategoryResponseDTO>>(
                    categories,
                    HttpStatusCode.OK,
                    true,
                    "Filtered food categories retrieved successfully."
                );
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while retrieving filtered food categories.", ex);
                throw new CustomInvalidOperationException("An error occurred while retrieving filtered food categories.");
            }
        }

        [HttpGet("all-categories-and-fooditems")]
        public async Task<BaseResponse<List<FoodCategoryResponseDTO>>> GetAllCategoriesAndFoodItemsAsync()
        {
            try
            {
                var categoriesWithItems = await _menuManagementService.GetAllCategoriesAndFoodItemsAsync();

                if (categoriesWithItems == null || categoriesWithItems.Count == 0)
                {
                    throw new NotFoundException("No categories found.");
                }

                return new BaseResponse<List<FoodCategoryResponseDTO>>
                    (categoriesWithItems,
                    HttpStatusCode.OK, 
                    true, 
                    "Categories with food items retrieved successfully.");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while retrieving categories with food items.", ex);
                throw new CustomInvalidOperationException("An error occurred while retrieving categories with food items.");
            }
        }

        //[Authorize]
        [HttpPost("add-food-item")]
        public async Task<BaseResponse<Guid>> AddFoodItem([FromForm] FoodItemRequestDTO foodItemDto)
        {
            try
            {
                if (string.IsNullOrEmpty(foodItemDto.Name) || string.IsNullOrEmpty(foodItemDto.Description) || foodItemDto.CategoryId == Guid.Empty)
                {
                    throw new CustomInvalidOperationException("All fields are required.");
                }
                if (foodItemDto.Price <= 0)
                {
                    throw new CustomInvalidOperationException("Price must be greater than zero.");
                }
              
                // Add the food item
                var response = await _menuManagementService.AddFoodItemAsync(foodItemDto, foodItemDto.CategoryId);

                // Return the success response
                return new BaseResponse<Guid>(
                    response._data,
                    HttpStatusCode.Created,
                    true,
                    "Food item added successfully."
                );
            }
            catch (DuplicateRecordException ex)
            {
                Log.Error("Item name already exists");
                throw;
            }
            catch (CustomInvalidOperationException ex)
            {
                Log.Error("Add Item failed");
                throw; // This will be caught by the middleware
            }
            catch (Exception ex)
            {
                throw new CustomInvalidOperationException("An error occurred while adding a food item.");
            }
        }


        [HttpGet("filter-food-items")]
        public async Task<BaseResponse<List<FoodItemResponseDTO>>> GetFoodItemsByIdOrName([FromQuery] Guid? itemId, [FromQuery] string name)
        {
            try
            {
                // Fetch the filtered food items from the service
                var foodItems = await _menuManagementService.GetFoodItemsByIdOrNameAsync(itemId, name);

                // Check if food items are empty
                if (foodItems == null || foodItems.Count == 0)
                {
                    throw new NotFoundException("No food items found matching the filter criteria.");
                }

                // Return the success response with filtered food items
                return new BaseResponse<List<FoodItemResponseDTO>>(
                    foodItems,
                    HttpStatusCode.OK,
                    true,
                    "Filtered food items retrieved successfully."
                );
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while retrieving filtered food items.", ex);
                throw new CustomInvalidOperationException("An error occurred while retrieving filtered food items.");
            }
        }

    }

}
