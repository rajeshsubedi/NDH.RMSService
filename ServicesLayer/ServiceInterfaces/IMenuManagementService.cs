using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.MenuManagementDTO;
using DomainLayer.Wrappers.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ServiceInterfaces
{
    public interface IMenuManagementService
    {
        Task<BaseResponse<Guid>> AddFoodCategoryAsync(FoodCategoryRequestDTO categoryDto, string imagePath);
        Task<FoodCategoryResponseDTO> GetAllFoodItemsWithCategoryId(Guid categoryId);
        Task<List<FoodCategoryResponseDTO>> GetAllFoodCategoriesOnlyAsync();
        Task<List<FoodCategoryResponseDTO>> GetFoodCategoriesByIdOrNameAsync(Guid? id, string name);
        Task<List<FoodCategoryandItemOnlyResponseDTO>> GetAllCategoriesAndFoodItemsAsync();
        Task<BaseResponse<Guid>> AddFoodItemAsync(FoodItemRequestDTO foodItemDto, Guid categoryId, string itemImagePath);
        Task<List<FoodItemResponseDTO>> GetAllFoodItemsAsync();
        Task<List<FoodItemResponseDTO>> GetFoodItemsByIdOrNameAsync(Guid? itemId, string name);

        Task<BaseResponse<Guid>> UpdateFoodCategoryAsync(UpdateFoodCategoryRequestDTO categoryDto, string imagePath);
        Task<BaseResponse<Guid>> DeleteFoodCategoryAsync(Guid categoryId);
        Task<BaseResponse<Guid>> UpdateFoodItemAsync(Guid itemId, FoodItemRequestDTO foodItemDto, string imagePath);
        Task<BaseResponse<bool>> DeleteFoodItemAsync(Guid itemId);
    }
}
