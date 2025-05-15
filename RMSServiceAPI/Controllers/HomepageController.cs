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
        private readonly IImageOperationService _imageOperationService;


        public HomePageController(IHomepageService homePageService, IImageOperationService imageOperationService)
        {
            _homePageService = homePageService ?? throw new ArgumentNullException(nameof(homePageService));
            _imageOperationService = imageOperationService;
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
        public async Task<BaseResponse<SpecialEventDetails>> AddSpecialEvent([FromForm] SpecialEventDTO specialEventDto, IFormFile imageFile)
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

                string imagePath = await _imageOperationService.SaveSpecialEventImageAsync(imageFile);
                var addedEvent = await _homePageService.AddSpecialEventAsync(specialEventDto, imagePath);

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

        // Get All Banners
        [HttpGet("get-banners")]
        public async Task<BaseResponse<List<BannerDetails>>> GetBanners()
        {
            try
            {
                var banners = await _homePageService.GetAllBannersAsync();
                return new BaseResponse<List<BannerDetails>>(
                                 banners,
                                 HttpStatusCode.OK,
                                 true,
                                 "GetBanners retrieved successfully."
                );
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while searching for GetBanners.", ex);
                throw new CustomInvalidOperationException($"An error occurred while searching for GetBanners.{ex.Message}");
            }

        }

        [HttpPost("add-banner")]
        public async Task<BaseResponse<string>> AddBanner([FromBody] BannerDetailsRequestDto banner, IFormFile imageFile)
        {
            try
            {

                if (banner == null || string.IsNullOrEmpty(banner.Name) )
                {
                    return new BaseResponse<string>(
                        null,
                        HttpStatusCode.BadRequest,
                        false,
                        "Invalid banner data."
                    );
                }
                string imagePath = await _imageOperationService.SaveBannerImageAsync(imageFile);
                await _homePageService.AddBannerAsync(banner, imagePath);

                return new BaseResponse<string>(
                    "Banner added successfully.",
                    HttpStatusCode.Created,
                    true,
                    "Banner added successfully."
                );
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while adding for AddBanner.", ex);
                throw new CustomInvalidOperationException($"An error occurred while adding for AddBanner.{ex.Message}");
            }
        }

        // ✅ GET - Get all company details
        [HttpGet("get-company-details")]
        public async Task<BaseResponse<List<CompanyDetails>>> GetCompanyDetails()
        {
            try
            {
                var companyDetails = await _homePageService.GetAllCompanyDetailsAsync();
                return new BaseResponse<List<CompanyDetails>>(
                    companyDetails,
                    HttpStatusCode.OK,
                    true,
                    "Company details retrieved successfully."
                );
            }
            catch (Exception ex)
            {
                Log.Error("Error fetching company details", ex);
                throw new CustomInvalidOperationException($"Error fetching company details: {ex.Message}");
            }
        }

        // ✅ POST - Add new company details
        [HttpPost("add-company-details")]
        public async Task<BaseResponse<string>> AddCompanyDetails([FromBody] CompanyDetailsRequestDto companyDto, IFormFile imageFile)
        {
            try
            {
                if (companyDto == null || string.IsNullOrEmpty(companyDto.Name))
                {
                    return new BaseResponse<string>(
                        null,
                        HttpStatusCode.BadRequest,
                        false,
                        "Invalid company data."
                    );
                }
                string imagePath = await _imageOperationService.SaveCompanyDetailsImageAsync(imageFile);
                await _homePageService.AddCompanyAsync(companyDto, imagePath);

                return new BaseResponse<string>(
                    "Company details added successfully.",
                    HttpStatusCode.Created,
                    true,
                    "Company details added successfully."
                );
            }
            catch (Exception ex)
            {
                Log.Error("Error adding company details", ex);
                throw new CustomInvalidOperationException($"Error adding company details: {ex.Message}");
            }
        }

        [HttpPut("update-specialgroup/{id}")]
        public async Task<BaseResponse<Guid>> UpdateHomepageSpecialGroup(Guid id, [FromBody] HomepageSpecialGroupDTO homepageSpecialGroupDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(homepageSpecialGroupDto.GroupName) ||
                    string.IsNullOrWhiteSpace(homepageSpecialGroupDto.GroupDescription))
                {
                    throw new CustomInvalidOperationException("All required fields must be provided.");
                }

                var updatedGroupId = await _homePageService.UpdateHomepageSpecialGroupAsync(id, homepageSpecialGroupDto);

                return new BaseResponse<Guid>(
                    updatedGroupId,
                    HttpStatusCode.OK,
                    true,
                    "Special group updated successfully"
                );
            }
            catch (NotFoundException)
            {
                Log.Error($"Special group with ID {id} not found for update.");
                throw;
            }
            catch (CustomInvalidOperationException)
            {
                Log.Error("Update special food group failed.");
                throw;
            }
            catch (Exception)
            {
                Log.Error("An error occurred while updating the special food group.");
                throw new CustomInvalidOperationException("An error occurred while updating the special food group.");
            }
        }

        [HttpPut("update-event/{eventId}")]
        public async Task<BaseResponse<SpecialEventDetails>> UpdateSpecialEvent(Guid eventId, [FromForm] SpecialEventDTO specialEventDto, IFormFile imageFile)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(specialEventDto.EventName) ||
                    string.IsNullOrWhiteSpace(specialEventDto.Location) ||
                    string.IsNullOrWhiteSpace(specialEventDto.Description))
                {
                    throw new CustomInvalidOperationException("All required properties must be provided.");
                }
                string imagePath = await _imageOperationService.SaveSpecialEventImageAsync(imageFile);
                var updatedEvent = await _homePageService.UpdateSpecialEventAsync(eventId, specialEventDto, imagePath);

                return new BaseResponse<SpecialEventDetails>(
                    updatedEvent,
                    HttpStatusCode.OK,
                    true,
                    "Special event updated successfully."
                );
            }
            catch (CustomInvalidOperationException)
            {
                Log.Error("Update special event failed.");
                throw;
            }
            catch (Exception)
            {
                Log.Error("An error occurred while updating the special event.");
                throw new CustomInvalidOperationException("An error occurred while updating the special event.");
            }
        }


        [HttpDelete("delete-specialgroup/{id}")]
        public async Task<BaseResponse<Guid>> DeleteHomepageSpecialGroup(Guid id)
        {
            try
            {
                var deletedGroupId = await _homePageService.DeleteHomepageSpecialGroupAsync(id);

                return new BaseResponse<Guid>(
                    deletedGroupId,
                    HttpStatusCode.OK,
                    true,
                    "Special group deleted successfully"
                );
            }
            catch (NotFoundException)
            {
                Log.Error($"Special group with ID {id} not found for deletion.");
                throw;
            }
            catch (Exception)
            {
                Log.Error("An error occurred while deleting the special food group.");
                throw new CustomInvalidOperationException("An error occurred while deleting the special food group.");
            }
        }

        [HttpDelete("delete-event/{eventId}")]
        public async Task<BaseResponse<Guid>> DeleteSpecialEvent(Guid eventId)
        {
            try
            {
                var deletedEventId = await _homePageService.DeleteSpecialEventAsync(eventId);

                return new BaseResponse<Guid>(
                    deletedEventId,
                    HttpStatusCode.OK,
                    true,
                    "Special event deleted successfully."
                );
            }
            catch (CustomInvalidOperationException)
            {
                Log.Error("Delete special event failed.");
                throw;
            }
            catch (Exception)
            {
                Log.Error("An error occurred while deleting the special event.");
                throw new CustomInvalidOperationException("An error occurred while deleting the special event.");
            }
        }

        [HttpPut("update-banner/{bannerId}")]
        public async Task<BaseResponse<string>> UpdateBanner(Guid bannerId, [FromBody] BannerDetailsRequestDto banner, IFormFile imageFile)
        {
            try
            {
                if (banner == null || string.IsNullOrEmpty(banner.Name))
                {
                    return new BaseResponse<string>(
                        null,
                        HttpStatusCode.BadRequest,
                        false,
                        "Invalid banner data."
                    );
                }
                string imagePath = await _imageOperationService.SaveBannerImageAsync(imageFile);
                await _homePageService.UpdateBannerAsync(bannerId, banner, imagePath);

                return new BaseResponse<string>(
                    "Banner updated successfully.",
                    HttpStatusCode.OK,
                    true,
                    "Banner updated successfully."
                );
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while updating the banner.", ex);
                throw new CustomInvalidOperationException($"An error occurred while updating the banner. {ex.Message}");
            }
        }

        [HttpDelete("delete-banner/{bannerId}")]
        public async Task<BaseResponse<Guid>> DeleteBanner(Guid bannerId)
        {
            try
            {
                var deletedBannerId = await _homePageService.DeleteBannerAsync(bannerId);

                return new BaseResponse<Guid>(
                    deletedBannerId,
                    HttpStatusCode.OK,
                    true,
                    "Banner deleted successfully."
                );
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while deleting the banner.", ex);
                throw new CustomInvalidOperationException($"An error occurred while deleting the banner. {ex.Message}");
            }
        }

        [HttpPut("update-company/{companyId}")]
        public async Task<BaseResponse<string>> UpdateCompany(Guid companyId, [FromBody] CompanyDetailsRequestDto companyDto, IFormFile imageFile)
        {
            try
            {
                if (companyDto == null || string.IsNullOrEmpty(companyDto.Name))
                {
                    return new BaseResponse<string>(
                        null,
                        HttpStatusCode.BadRequest,
                        false,
                        "Invalid company data."
                    );
                }
                string imagePath = await _imageOperationService.SaveCompanyDetailsImageAsync(imageFile);
                await _homePageService.UpdateCompanyAsync(companyId, companyDto, imagePath);

                return new BaseResponse<string>(
                    "Company details updated successfully.",
                    HttpStatusCode.OK,
                    true,
                    "Company details updated successfully."
                );
            }
            catch (Exception ex)
            {
                Log.Error("Error updating company details", ex);
                throw new CustomInvalidOperationException($"Error updating company details: {ex.Message}");
            }
        }

        [HttpDelete("delete-company/{companyId}")]
        public async Task<BaseResponse<Guid>> DeleteCompany(Guid companyId)
        {
            try
            {
                var deletedCompanyId = await _homePageService.DeleteCompanyAsync(companyId);

                return new BaseResponse<Guid>(
                    deletedCompanyId,
                    HttpStatusCode.OK,
                    true,
                    "Company deleted successfully."
                );
            }
            catch (Exception ex)
            {
                Log.Error("Error deleting company", ex);
                throw new CustomInvalidOperationException($"Error deleting company: {ex.Message}");
            }
        }

    }
}
