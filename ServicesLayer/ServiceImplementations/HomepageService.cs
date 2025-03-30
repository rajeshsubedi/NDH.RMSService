using DataAccessLayer.Infrastructure.Repositories.RepoInterfaces;
using DomainLayer.Models.DataModels.HomepageManagementModels;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.HomepageManagementDTO;
using DomainLayer.Wrappers.DTO.MenuManagementDTO;
using ServicesLayer.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ServiceImplementations
{
    public class HomepageService : IHomepageService
    {
        private readonly IHomepageRepo _repository;

        public HomepageService(IHomepageRepo repository)
        {
            _repository = repository;
        }

        public async Task<HomepageSpecialGroups> AddHomepageSpecialGroupAsync(HomepageSpecialGroupDTO homepageSpecialGroupDto)
        {
            if (homepageSpecialGroupDto == null)
            {
                throw new ArgumentNullException(nameof(homepageSpecialGroupDto));
            }

            var homepageSpecialGroup = new HomepageSpecialGroups
            {
                GroupId = Guid.NewGuid(),
                GroupName = homepageSpecialGroupDto.GroupName,
                GroupDescription = homepageSpecialGroupDto.GroupDescription,
                GroupType = homepageSpecialGroupDto.GroupType,
                StartDate = homepageSpecialGroupDto.StartDate,
                EndDate = homepageSpecialGroupDto.EndDate,
                Status = homepageSpecialGroupDto.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = homepageSpecialGroupDto.CreatedBy,
                UpdatedBy = homepageSpecialGroupDto.UpdatedBy,
                ImageUrl = homepageSpecialGroupDto.ImageUrl
            };

            await _repository.AddHomepageSpecialGroupAsync(homepageSpecialGroup);

            return homepageSpecialGroup;
        }

        public async Task<List<HomepageSpecialGroups>> GetAllHomepageSpecialGroupsAsync()
        {
            return await _repository.GetAllHomepageSpecialGroupsAsync();
        }

        public async Task<IEnumerable<MenuItemDetails>> GetSpecialOffersAsync()
        {
            return await _repository.GetSpecialOffersAsync();
        }

        public async Task<IEnumerable<SpecialEventDetails>> GetSpecialEventsAsync()
        {
            return await _repository.GetSpecialEventsAsync();
        }
        public async Task<SpecialEventDetails> AddSpecialEventAsync(SpecialEventDTO specialEventDto)
        {
            if (specialEventDto == null)
            {
                throw new ArgumentNullException(nameof(specialEventDto));
            }
            var specialEventDetails = new SpecialEventDetails();
            specialEventDetails.EventId = Guid.NewGuid();
            specialEventDetails.EventName = specialEventDto.EventName;
            specialEventDetails.EventDate = DateTime.Now;
            specialEventDetails.Description = specialEventDto.Description;
            specialEventDetails.Location = specialEventDto.Location;
            specialEventDetails.ImageUrl = specialEventDto.ImageUrl;
            specialEventDetails.ImagePath = specialEventDto.ImagePath;
            await _repository.AddSpecialEventAsync(specialEventDetails);

            return specialEventDetails;
        }

        public async Task<List<MenuItemDetails>> SearchFoodItemsAsync(string name, string description)
        {
            var foodItems = await _repository.SearchFoodItemsAsync(name, description);
            return foodItems;
        }
    }
}
