using DomainLayer.Models.DataModels.HomepageManagementModels;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.HomepageManagementDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Repositories.RepoInterfaces
{
    public interface IHomepageRepo
    {
        Task AddHomepageSpecialGroupAsync(HomepageSpecialGroup homepageSpecialGroup);
        Task<HomepageSpecialGroup> GetHomepageSpecialGroupByNameAsync(string groupName);
        Task<List<HomepageSpecialGroup>> GetAllHomepageSpecialGroupsAsync();
        Task<IEnumerable<SpecialEventDetails>> GetSpecialEventsAsync();
        Task AddSpecialEventAsync(SpecialEventDetails specialEventDetails);
        Task<List<MenuItemDetails>> SearchFoodItemsAsync(string name, string description);
        Task<List<BannerDetails>> GetAllBannerDetailsAsync();
        Task AddBannerDetailsAsync(BannerDetails entity);
        Task<List<CompanyDetails>> GetAllAGetAllCompanyDetailsAsyncsync();
        Task AddCompanyDetailsAsync(CompanyDetails entity);
    }
}
