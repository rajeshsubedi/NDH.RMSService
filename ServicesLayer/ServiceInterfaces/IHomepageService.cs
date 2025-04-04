using DomainLayer.Models.DataModels.HomepageManagementModels;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.HomepageManagementDTO;
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
        Task<Guid> AddHomepageSpecialGroupAsync(HomepageSpecialGroupDTO homepageSpecialGroupDto);

        Task<List<HomepageSpecialGroupResponseDTO>> GetAllHomepageSpecialGroupsAsync();
        Task<IEnumerable<SpecialEventDetails>> GetSpecialEventsAsync();
        Task<SpecialEventDetails> AddSpecialEventAsync(SpecialEventDTO specialEventDto);
        Task<List<MenuItemDetails>> SearchFoodItemsAsync(string name, string description);
        Task<List<BannerDetails>> GetAllBannersAsync();
        Task AddBannerAsync(BannerDetailsRequestDto banner);

        Task<List<CompanyDetails>> GetAllCompanyDetailsAsync();
        Task AddCompanyAsync(CompanyDetailsRequestDto companyDto);
    }
}
