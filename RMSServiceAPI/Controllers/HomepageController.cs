using DomainLayer.Exceptions;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.MenuManagementDTO;
using DomainLayer.Wrappers.GlobalResponse;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ServicesLayer.ServiceImplementations;
using ServicesLayer.ServiceInterfaces;
using System.Net;

namespace RMSServiceAPI.Controllers
{
    public class HomePageController : ControllerBase
    {

        private readonly IHomepageService _homePageService;

        public HomePageController(IHomepageService homePageService)
        {
            _homePageService = homePageService ?? throw new ArgumentNullException(nameof(homePageService));
        }

        [HttpGet("special-offers")]
        public async Task<BaseResponse<IEnumerable<MenuItemDetails>>> GetSpecialOffers()
        {
            try
            {
                var specialOffers = await _homePageService.GetSpecialOffersAsync();

                var baseResponse = new BaseResponse<IEnumerable<MenuItemDetails>>(
                    specialOffers,
                    HttpStatusCode.OK,
                    true,
                    "Special offers retrieved successfully"
                );

                return baseResponse;
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while fetching special offers.");
                throw new CustomInvalidOperationException("An error occurred while fetching special offers.");
            }
        }

        [HttpGet("special-events")]
        public async Task<BaseResponse<IEnumerable<SpecialEventDetails>>> GetSpecialEvents()
        {
            try
            {
                var specialEvents = await _homePageService.GetSpecialEventsAsync();

                var baseResponse = new BaseResponse<IEnumerable<SpecialEventDetails>>(
                    specialEvents,
                    HttpStatusCode.OK,
                    true,
                    "Special events retrieved successfully"
                );

                return baseResponse;
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while fetching special events.");
                throw new CustomInvalidOperationException("An error occurred while fetching special events.");
            }
        }

        [HttpPost("add-event")]
        public async Task<BaseResponse<SpecialEventDetails>> AddSpecialEvent([FromForm] SpecialEventDTO specialEventDto)
        {
            try
            {
                byte[] imageBytes = Array.Empty<byte>(); // Creates an empty byte array.

                // Ensure the special event details are valid
                if (specialEventDto == null)
                {
                    throw new CustomInvalidOperationException("Special event details cannot be null.");
                }
                if (specialEventDto.ImageData != null && specialEventDto.ImageData.Length > 0)
                {
                    // Process the image file
                    using (var stream = new MemoryStream())
                    {
                        await specialEventDto.ImageData.CopyToAsync(stream);
                        imageBytes = stream.ToArray();
                        // Handle the byte array (e.g., save to file or database)
                    }
                }
                // Validate that all required properties are not null or empty
                if (string.IsNullOrWhiteSpace(specialEventDto.EventName) ||
                    string.IsNullOrWhiteSpace(specialEventDto.Location) ||
                    string.IsNullOrWhiteSpace(specialEventDto.Description))
                {
                    throw new CustomInvalidOperationException("All required properties must be provided.");
                }

                var addedEvent = await _homePageService.AddSpecialEventAsync(specialEventDto, imageBytes);

                var baseResponse = new BaseResponse<SpecialEventDetails>(
                    addedEvent,
                    HttpStatusCode.Created,
                    true,
                    "Special event added successfully"
                );

                return baseResponse;
            }
            catch (CustomInvalidOperationException)
            {
                Log.Error("Add Categories failed");
                throw; // This will be caught by the middleware
            }
            catch (Exception)
            {
                Log.Error("An error occurred while adding the special event.");
                throw new CustomInvalidOperationException("An error occurred while adding the special event.");
            }
        }

        [HttpGet("search")]
        public async Task<BaseResponse<List<MenuItemDetails>>> SearchFoodItems([FromQuery] string name, [FromQuery] string description)
        {
            try
            {
                // Fetch the filtered food items from the service
                var foodItems = await _homePageService.SearchFoodItemsAsync(name, description);

                // Check if any food items were found
                if (foodItems == null || foodItems.Count == 0)
                {
                    throw new NotFoundException("No food items found matching the search criteria.");
                }

                // Return a success response with the filtered food items
                return new BaseResponse<List<MenuItemDetails>>(
                    foodItems,
                    HttpStatusCode.OK,
                    true,
                    "Food items retrieved successfully."
                );
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while searching for food items.", ex);
                throw new CustomInvalidOperationException("An error occurred while searching for food items.");
            }
        }

    }
}
