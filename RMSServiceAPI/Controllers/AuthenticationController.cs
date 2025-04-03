using Azure;
using DomainLayer.Exceptions;
using DomainLayer.Wrappers.DTO.AuthenticationDTO;
using DomainLayer.Wrappers.GlobalResponse;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Serilog;
using ServicesLayer.ServiceInterface;
using ServicesLayer.ServiceInterfaces;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RMSServiceAPI.Controllers
{
    [ApiController]
    //[Route("api/[controller]")] 
    [Route("api/Auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserAuthenticationService _userAuthService;
        private readonly IEmailService _emailService;

        public AuthenticationController(IConfiguration configuration, IUserAuthenticationService userAuthService, IEmailService emailService)
        {
            _configuration = configuration;
            _userAuthService = userAuthService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<BaseResponse<UserLoginResponseDTO>> Login([FromBody] LoginRequestDTO loginModelDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(loginModelDTO.UserEmail) || string.IsNullOrEmpty(loginModelDTO.Password))
                {
                    throw new CustomInvalidOperationException("All fields are required.");
                }
                Log.Information("Logging User");
                var response = await _userAuthService.AuthenticateLogin(loginModelDTO);

                var jwtToken = response.JwtToken;
                response.JwtToken = null;
                var baseResponse = new BaseResponse<UserLoginResponseDTO>
                {
                    _success = true,
                    _message = "Login Successful",
                    _statusCode = HttpStatusCode.OK,
                    _data = response
                };

                // Set the JWT token in the response header
                Response.Headers.Add("Authorization", $"Bearer {jwtToken}");
                return baseResponse;
            }
            catch (UserUnauthenticatedException ex)
            {
                Log.Error("User authentication failed");
                throw; // This will be caught by the middleware
            }
            catch (CustomInvalidOperationException ex)
            {
                Log.Error("User authentication failed");
                throw; // This will be caught by the middleware
            }
            catch (Exception ex)
            {
                Log.Error("Unexpected error during login");
                throw new UserUnauthenticatedException("Access Denied");
            }
        }

        [HttpPost("register")]
        public async Task<BaseResponse<bool>> Register([FromBody] UserRegistrationRequestDTO model)
        {
            try
            {
                // Validate the model fields manually
                if (string.IsNullOrWhiteSpace(model.Username) ||
                    string.IsNullOrWhiteSpace(model.Password) ||
                    string.IsNullOrWhiteSpace(model.Email) ||
                    string.IsNullOrWhiteSpace(model.PhoneNumber))
                {
                    throw new CustomInvalidOperationException("All fields are required.");
                }
                var result = await _userAuthService.StoreUnregisterdUserForVerificationAsync(model);
                if (result)
                {
                    var user = await _userAuthService.GetUserByEmailOnlyAsync(model.Email);
                    if (user == null)
                    {
                        throw new NotFoundException("User Not exist with this email.");
                    }
                    var emailToken = _userAuthService.GeneratJWTToken(user);

                    var baseUrl = _configuration["ApplicationBaseURLS:RMSBaseUrl"];
                    var verificationUrl = $"{baseUrl}/api/Auth/verify-email?id={emailToken}";

                    // Store the verification token in the database
                    await _userAuthService.StoreEmailVerificationToken(model.Email, emailToken);

                    string emailBody = $@"
                                    <html>
                                    <body>
                                    <p>Dear Sir/Mam,</p>
                                    <p>Thank you for registering. Please click the link below to verify your email address:</p>
                                    <p><a href=""{verificationUrl}"">Verify your email address</a></p>
                                    <p>Thank you,<br />Hami Tech Group</p>
                                    </body>
                                    </html>
                                    ";

                    // Send verification email
                    await _emailService.SendEmailAsync(model.Email, "RMS Email Verification", emailBody);

                    BaseResponse<bool> baseResponse = new BaseResponse<bool>();
                    baseResponse._success = true;
                    baseResponse._message = "User registration started. Please check your email to verify your account.";
                    baseResponse._statusCode = HttpStatusCode.OK;
                    baseResponse._data = result;
                    return baseResponse;
                }
                else
                {
                    throw new DuplicateRecordException();
                }
            }
            catch (DuplicateRecordException ex)
            {
                throw;
            }
            catch (CustomInvalidOperationException ex)
            {
                throw;
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomInvalidOperationException(ex.Message);
            }
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string id)
        {
            // Remove curly braces and parse the GUID
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Invalid or missing token.");
            }
            try
            {
                // Verify the email using the provided token
                var result = await _userAuthService.VerifyEmailAsync(id);
                if (result)
                {
                    var clientUrl = _configuration["ApplicationBaseURLS:RMSClientUrl"];
                    var redirectUrl = $"{clientUrl}";
                    //var redirectUrl = $"https://www.youtube.com/";
                    Response.Headers.Add("IsVerified", $"true");
                    return Redirect(redirectUrl);
                }
                else
                {
                    throw new CustomInvalidOperationException("Invalid token.");
                }
            }
            catch (CustomInvalidOperationException)
            {
                // Rethrow the custom exception
                throw;
            }
            catch (NotFoundException)
            {
                // Rethrow the custom exception
                throw;
            }
            catch (Exception ex)
            {
                // Log and throw generic exceptions
                Log.Error(ex, "An error occurred while verifying the email.");
                throw new CustomInvalidOperationException("An error occurred while verifying the email.");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<BaseResponse<UserLoginResponseDTO>> RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequest)
        {
            try
            {
                if (refreshTokenRequest == null ||
                    refreshTokenRequest.UserId == Guid.Empty ||
                    string.IsNullOrWhiteSpace(refreshTokenRequest.RefreshToken) ||
                    refreshTokenRequest.TokenExpiration == default)
                {
                    throw new CustomInvalidOperationException("All fields are required.");
                }
                var userResponse = await _userAuthService.RefreshJwtUserToken(refreshTokenRequest);
                var jwtToken = userResponse.JwtToken;
                userResponse.JwtToken = null;

                var baseResponse = new BaseResponse<UserLoginResponseDTO>
                {
                    _success = true,
                    _message = "RefreshToken Successful",
                    _statusCode = HttpStatusCode.OK,
                    _data = userResponse
                };
                // Add the properties to the response headers
                Response.Headers.Add("Authorization", $"Bearer {jwtToken}");
                return baseResponse;
            }
            catch (CustomInvalidOperationException ex)
            {
                throw new CustomInvalidOperationException("Invalid or expired refresh token");

            }
            catch (Exception ex)
            {
                throw new NotFoundException("An error occurred while processing your refresh token request");
            }
        }

        [HttpPost("signout")]
        public async Task<BaseResponse<bool>> SignOut([FromBody] RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            try
            {
                if (refreshTokenRequestDTO == null ||
                   refreshTokenRequestDTO.UserId == Guid.Empty ||
                   string.IsNullOrWhiteSpace(refreshTokenRequestDTO.RefreshToken) ||
                   refreshTokenRequestDTO.TokenExpiration == default)
                {
                    throw new CustomInvalidOperationException("All fields are required.");
                }
                var result = await _userAuthService.SignOutAsync(refreshTokenRequestDTO);
                if (result)
                {
                    BaseResponse<bool> baseResponse = new BaseResponse<bool>();
                    baseResponse._success = true;
                    baseResponse._message = "SignOut successfully";
                    baseResponse._statusCode = HttpStatusCode.OK;
                    baseResponse._data = result;
                    return baseResponse;
                }
                else
                {
                    throw new NotFoundException();
                }
            }
            catch (NotFoundException ex)
            {
                // Re-throw custom exceptions to be handled by the calling method
                Log.Error("{ex}", ex);
                throw new NotFoundException("Invalid Signout Request");
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomInvalidOperationException("An error occurred while invalidating the refresh token");
            }
        }


        [HttpPost("forgot-password")]
        public async Task<BaseResponse<bool>> ForgotPassword([FromBody] ForgotPasswordRequestDTO forgotPasswordRequestDTO)
        {
            try
            {
                // Validate the model fields manually
                if (string.IsNullOrWhiteSpace(forgotPasswordRequestDTO.UserEmail))
                {
                    throw new CustomInvalidOperationException("Email is required.");
                }
                var user = await _userAuthService.GetUserByEmailOnlyAsync(forgotPasswordRequestDTO.UserEmail);
                if (user == null)
                {
                    throw new NotFoundException("User Not exist with this email.");
                }

                var baseUrl = _configuration["ApplicationBaseURLS:RMSBaseUrl"];
                var verificationUrl = $"{baseUrl}/api/Auth/verify-reset-email-pw";


                string emailBody = $@"
                                    <html>
                                    <body>
                                    <p>Dear Sir/Mam,</p>
                                    <p>Please click the link below to reset your password:</p>
                                    <p><a href=""{verificationUrl}"">Click to reset your password</a></p>
                                    <p>Thank you,<br />Hami Tech Group</p>
                                    </body>
                                    </html>
                                    ";

                // Send verification email
                await _emailService.SendEmailAsync(forgotPasswordRequestDTO.UserEmail, "RMS Email Verification", emailBody);
                BaseResponse<bool> baseResponse = new BaseResponse<bool>();
                baseResponse._success = true;
                baseResponse._message = "Password Reset started. Please check your email to verify your account.";
                baseResponse._statusCode = HttpStatusCode.OK;
                baseResponse._data = true;
                return baseResponse;

            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (CustomInvalidOperationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("confirm-otp")]
        public async Task<BaseResponse<Guid>> ConfirmOtp([FromBody] ConfirmOtpRequest model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) || model.Otp <= 0)
                {
                    throw new CustomInvalidOperationException("Email and OTP are required.");
                }

                var result = await _userAuthService.ConfirmPasswordResetOtpAsync(model.Email, model.Otp);

                if (result)
                {
                    // Assume you have a method to get user details by email
                    var user = await _userAuthService.GetUserByEmaileAndConfirmFlagLogin(model.Email);
                    var token = _userAuthService.GeneratJWTToken(user);

                    // Add the JWT token to the response headers
                    Response.Headers.Add("Authorization", $"Bearer {token}");

                    return new BaseResponse<Guid>
                    {
                        _data = user.UserId,
                        _success = result,
                        _message = "OTP confirmed. You can now reset your password.",
                        _statusCode = HttpStatusCode.OK
                    };
                }
                else
                {
                    throw new CustomInvalidOperationException("Invalid OTP or OTP expired.");
                }
            }
            catch (CustomInvalidOperationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomInvalidOperationException(ex.Message);
            }
        }



        [HttpPost("reset-password")]
        public async Task<BaseResponse<bool>> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    throw new CustomInvalidOperationException("Email and new password are required.");
                }

                // Call service to reset the password
                var result = await _userAuthService.ResetPasswordAsync(model.Email, model.NewPassword);

                if (result)
                {
                    // Assume you have a method to get user details by email
                    var user = await _userAuthService.GetUserByEmaileAndConfirmFlagLogin(model.Email);
                    var token = _userAuthService.GeneratJWTToken(user);

                    // Add the JWT token to the response headers
                    Response.Headers.Add("Authorization", $"Bearer {token}");

                    return new BaseResponse<bool>
                    {
                        _success = result,
                        _message = "Password reset successfully.",
                        _statusCode = HttpStatusCode.OK,
                        _data = result
                    };
                }
                else
                {
                    throw new CustomInvalidOperationException("Password reset failed, Wrong Email.");
                }

            }
            catch (CustomInvalidOperationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CustomInvalidOperationException(ex.Message);
            }
        }


        [HttpGet("verify-reset-email-pw")]
        public async Task<IActionResult> VerifyResetPasswordEmail()
        {
            try
            {
                    var clientUrl = _configuration["ApplicationBaseURLS:RMSClientUrl"];
                    var redirectUrl = $"{clientUrl}/reset-password";
                    //var redirectUrl = $"https://www.youtube.com/";
                    return Redirect(redirectUrl);
            }
            catch (CustomInvalidOperationException)
            {
                // Rethrow the custom exception
                throw;
            }
            catch (NotFoundException)
            {
                // Rethrow the custom exception
                throw;
            }
            catch (Exception ex)
            {
                // Log and throw generic exceptions
                Log.Error(ex, "An error occurred while reseting the email.");
                throw new CustomInvalidOperationException("An error occurred while reseting the email.");
            }
        }

        [HttpGet("users")]
        public async Task<BaseResponse<List<UserDetailsResponseDTO>>> GetAllUsers()
        {
            try
            {
                Log.Information("Fetching all users");
                var users = await _userAuthService.GetAllUsersAsync();

                if (users == null || !users.Any())
                {
                    return new BaseResponse<List<UserDetailsResponseDTO>>
                    {
                        _success = false,
                        _message = "No users found",
                        _statusCode = HttpStatusCode.NotFound,
                        _data = null
                    };
                }

                return new BaseResponse<List<UserDetailsResponseDTO>>
                {
                    _success = true,
                    _message = "Users retrieved successfully",
                    _statusCode = HttpStatusCode.OK,
                    _data = users
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error while fetching user details");
                return new BaseResponse<List<UserDetailsResponseDTO>>
                {
                    _success = false,
                    _message = "An error occurred while fetching user details",
                    _statusCode = HttpStatusCode.InternalServerError,
                    _data = null
                };
            }
        }


        [HttpDelete("users/{userId}")]
        public async Task<BaseResponse<Guid>> DeleteUser(Guid userId)
        {
            try
            {
                Log.Information("Deleting user with ID: {UserId}", userId);
                var result = await _userAuthService.DeleteUserAsync(userId);

                if (!result)
                {
                    return new BaseResponse<Guid>
                    {
                        _success = false,
                        _message = "User not found or could not be deleted",
                        _statusCode = HttpStatusCode.NotFound,
                        _data = Guid.NewGuid()
                    };
                }

                return new BaseResponse<Guid>
                {
                    _success = true,
                    _message = "User deleted successfully",
                    _statusCode = HttpStatusCode.OK,
                    _data = userId
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unexpected error while deleting user");
                return new BaseResponse<Guid>
                {
                    _success = false,
                    _message = "An error occurred while deleting the user",
                    _statusCode = HttpStatusCode.InternalServerError,
                    _data = Guid.NewGuid()
                };
            }
        }
    }

}
