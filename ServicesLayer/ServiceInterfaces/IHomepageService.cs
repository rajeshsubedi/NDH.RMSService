using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.MenuManagementDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ServiceInterfaces
{
    // IHomePageService.cs
    public interface IHomepageService
    {
        Task<IEnumerable<MenuItemDetails>> GetSpecialOffersAsync();
        Task<IEnumerable<SpecialEventDetails>> GetSpecialEventsAsync();
        Task<SpecialEventDetails> AddSpecialEventAsync(SpecialEventDTO specialEventDto, byte[] imageByte);
        Task<List<MenuItemDetails>> SearchFoodItemsAsync(string name, string description);

    }
}
