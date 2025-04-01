using DomainLayer.Exceptions;
using DomainLayer.Models.DataModels.HomepageManagementModels;
using DomainLayer.Models.DataModels.MenuManagementModels;
using DomainLayer.Wrappers.DTO.HomepageManagementDTO;
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

        // POST: api/HomepageSpecialGroups/add
        [HttpPost("add-specialgroup")]
        public async Task<BaseResponse<Guid>> AddHomepageSpecialGroup([FromBody] HomepageSpecialGroupDTO homepageSpecialGroupDto)
        {
            try
            {
                // Validate input data
                if (string.IsNullOrWhiteSpace(homepageSpecialGroupDto.GroupName) ||
                    string.IsNullOrWhiteSpace(homepageSpecialGroupDto.GroupDescription))
                {
                    throw new CustomInvalidOperationException("All required fields must be provided.");
                }

                var addedGroup = await _homePageService.AddHomepageSpecialGroupAsync(homepageSpecialGroupDto);

                var baseResponse = new BaseResponse<Guid>(
                    addedGroup,
                    HttpStatusCode.Created,
                    true,
                    "Special group added successfully"
                );

                return baseResponse;
            }
            catch (CustomInvalidOperationException)
            {
                Log.Error("Add special food group failed");
                throw; // This will be caught by the middleware
            }
            catch (Exception)
            {
                Log.Error("An error occurred while adding the special food group.");
                throw new CustomInvalidOperationException("An error occurred while adding the special food group.");
            }
        }

        // GET: api/HomepageSpecialGroups
        [HttpGet("get-specialgroup")]
        public async Task<BaseResponse<List<HomepageSpecialGroupResponseDTO>>> GetAllHomepageSpecialGroups()
        {
            try
            {
                var specialGroups = await _homePageService.GetAllHomepageSpecialGroupsAsync();

                var baseResponse = new BaseResponse<List<HomepageSpecialGroupResponseDTO>>(
                    specialGroups,
                    HttpStatusCode.OK,
                    true,
                    "Special food groups fetched successfully"
                );

                return baseResponse;
            }
            catch (Exception)
            {
                Log.Error("An error occurred while fetching the special food groups.");
                throw new CustomInvalidOperationException("An error occurred while fetching the special food groups.");
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
                // Validate that all required properties are not null or empty
                if (string.IsNullOrWhiteSpace(specialEventDto.EventName) ||
                    string.IsNullOrWhiteSpace(specialEventDto.Location) ||
                    string.IsNullOrWhiteSpace(specialEventDto.Description))
                {
                    throw new CustomInvalidOperationException("All required properties must be provided.");
                }

                var addedEvent = await _homePageService.AddSpecialEventAsync(specialEventDto);

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
