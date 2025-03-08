using DomainLayer.Models.DataModels.AuthenticationModels;
using DomainLayer.Wrappers.DTO.AuthenticationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLayer.ServiceInterface
{
    public interface IUserAuthenticationService
    {
        Task<UserLoginResponseDTO> AuthenticateLogin(LoginRequestDTO loginModelDTO);
        Task<bool> StoreUnregisterdUserForVerificationAsync(UserRegistrationRequestDTO userRegistrationDetailsDTO);
        Task<bool> VerifyEmailAsync(string emailToken);
        Task<UserLoginResponseDTO> RefreshJwtUserToken(RefreshTokenRequestDTO refreshTokenRequestDTO);
        Task<bool> SignOutAsync(RefreshTokenRequestDTO refreshTokenRequestDTO);
        Task StoreEmailVerificationToken(string email, string emailToken);
        Task<bool> SendPasswordResetOtpAsync(string userEmail);
        Task<bool> ConfirmPasswordResetOtpAsync(string email, int otp);
        Task<UserRegistrationDetails> GetUserByEmaileAndConfirmFlagLogin(string email);
        Task<UserRegistrationDetails> GetUserByEmailOnlyAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string newPassword);
        string GeneratJWTToken(UserRegistrationDetails user);
    }
}
