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
        Task<BaseResponse<Guid>> AddFoodCategoryAsync(FoodCategoryRequestDTO categoryDto);
        Task<List<FoodCategoryResponseDTO>> GetAllFoodItemsWithCategoryId(Guid categoryId);
        Task<List<FoodCategoryResponseDTO>> GetFoodCategoriesByIdOrNameAsync(Guid? id, string name);
        Task<List<FoodCategoryResponseDTO>> GetAllCategoriesAndFoodItemsAsync();
        Task<BaseResponse<Guid>> AddFoodItemAsync(FoodItemRequestDTO foodItemDto, Guid categoryId);
        Task<List<FoodItemResponseDTO>> GetAllFoodItemsAsync();
        Task<List<FoodItemResponseDTO>> GetFoodItemsByIdOrNameAsync(Guid? itemId, string name);
    }
}
