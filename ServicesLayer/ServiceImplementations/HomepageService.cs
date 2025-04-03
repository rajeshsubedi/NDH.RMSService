using DataAccessLayer.Infrastructure.Repositories.RepoInterfaces;
using DomainLayer.Exceptions;
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

        public async Task<Guid> AddHomepageSpecialGroupAsync(HomepageSpecialGroupDTO homepageSpecialGroupDto)
        {
            if (homepageSpecialGroupDto == null)
            {
                throw new ArgumentNullException(nameof(homepageSpecialGroupDto), "The provided HomepageSpecialGroupDto cannot be null.");
            }

            // Check if a special group with the same name already exists (optional, based on your business rules)
            var existingGroup = await _repository.GetHomepageSpecialGroupByNameAsync(homepageSpecialGroupDto.GroupName);
            if (existingGroup != null)
            {
                throw new CustomInvalidOperationException($"A special group with the name '{homepageSpecialGroupDto.GroupName}' already exists.");
            }

            // Create the HomepageSpecialGroup entity
            var homepageSpecialGroup = new HomepageSpecialGroup
            {
                GroupId = Guid.NewGuid(),
                GroupName = homepageSpecialGroupDto.GroupName,
                GroupDescription = homepageSpecialGroupDto.GroupDescription,
            };

            // Add the new group to the database
            await _repository.AddHomepageSpecialGroupAsync(homepageSpecialGroup);

            // Return the GroupId of the newly created special group
            return homepageSpecialGroup.GroupId;
        }


        public async Task<List<HomepageSpecialGroupResponseDTO>> GetAllHomepageSpecialGroupsAsync()
        {
            var specialGroups = await _repository.GetAllHomepageSpecialGroupsAsync();

            // ✅ Map to DTOs using LINQ
            var specialGroupDtos = specialGroups.Select(group => new HomepageSpecialGroupResponseDTO
            {
                Id = group.GroupId,
                GroupName = group.GroupName,
                GroupDescription = group.GroupDescription,
            }).ToList();

            return specialGroupDtos;
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
