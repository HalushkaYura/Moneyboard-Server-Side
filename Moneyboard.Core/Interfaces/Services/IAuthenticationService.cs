using Microsoft.AspNetCore.Mvc;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.UserEntity;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task RegistrationAsync(User user, string password, string roleName);
        Task<UserAutorizationDTO> LoginAsync(string email, string password);


        Task<UserAutorizationDTO> RefreshTokenAsync(UserAutorizationDTO userTokensDTO);
        Task LogoutAsync(UserAutorizationDTO userTokensDTO);
        Task<UserAutorizationDTO> LoginTwoStepAsync(UserTwoFactorDTO twoFactorDTO);
        Task SentResetPasswordTokenAsync(string userEmail);
        Task ResetPasswordAsync(UserChangePasswordDTO userChangePasswordDTO);
        Task<UserAuthResponseDTO> ExternalLoginAsync(UserExternalAuthDTO authDTO);
        Task<User> GetAllUserEmailsAsync(string email);


        //-----------------------------------------------------------------------------

    }
}
