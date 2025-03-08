using DataAccessLayer.Infrastructure.Repositories.RepoInterfaces;
using DomainLayer.Models.DataModels.MenuManagementModels;
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

        public async Task<IEnumerable<MenuItemDetails>> GetSpecialOffersAsync()
        {
            return await _repository.GetSpecialOffersAsync();
        }

        public async Task<IEnumerable<SpecialEventDetails>> GetSpecialEventsAsync()
        {
            return await _repository.GetSpecialEventsAsync();
        }
        public async Task<SpecialEventDetails> AddSpecialEventAsync(SpecialEventDTO specialEventDto, byte[] imagebyte)
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
            specialEventDetails.ImageData = imagebyte;
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
