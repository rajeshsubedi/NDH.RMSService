using DomainLayer.Models.DataModels.HomepageManagementModels;
using DomainLayer.Models.DataModels.MenuManagementModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Repositories.RepoInterfaces
{
    public interface IHomepageRepo
    {
        Task AddHomepageSpecialGroupAsync(HomepageSpecialGroups homepageSpecialGroup);
        Task<List<HomepageSpecialGroups>> GetAllHomepageSpecialGroupsAsync();
        Task<IEnumerable<MenuItemDetails>> GetSpecialOffersAsync();
        Task<IEnumerable<SpecialEventDetails>> GetSpecialEventsAsync();
        Task AddSpecialEventAsync(SpecialEventDetails specialEventDetails);
        Task<List<MenuItemDetails>> SearchFoodItemsAsync(string name, string description);
    }
}
