﻿using DomainLayer.Models.DataModels.HomepageManagementModels;
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
        Task<HomepageSpecialGroups> AddHomepageSpecialGroupAsync(HomepageSpecialGroupDTO homepageSpecialGroupDto);

        Task<List<HomepageSpecialGroups>> GetAllHomepageSpecialGroupsAsync();
        Task<IEnumerable<MenuItemDetails>> GetSpecialOffersAsync();
        Task<IEnumerable<SpecialEventDetails>> GetSpecialEventsAsync();
        Task<SpecialEventDetails> AddSpecialEventAsync(SpecialEventDTO specialEventDto);
        Task<List<MenuItemDetails>> SearchFoodItemsAsync(string name, string description);

    }
}
