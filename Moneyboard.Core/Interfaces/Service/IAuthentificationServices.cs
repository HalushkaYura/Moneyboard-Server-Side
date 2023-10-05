using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.UserEntity;

namespace Moneyboard.Core.Interfaces.Service
{
    public interface IAuthentificationServices
    {
        Task RegistrationAsync(User user, string password, string roleName);
        Task<UserAutorizationDTO> LoginAsync(string email, string password);
        // Task<UserAutorizationDTO> RefreshTokenAsync(UserAutorizationDTO userTokensDTO);
        Task LogoutAsync();
        // Task<UserAutorizationDTO> LoginTwoStepAsync(UserTwoFactorDTO twoFactorDTO);
        // Task SentResetPasswordTokenAsync(string userEmail);
        // Task ResetPasswordAsync(UserChangePasswordDTO userChangePasswordDTO);
        //mTask<UserAuthResponseDTO> ExternalLoginAsync(UserExternalAuthDTO authDTO);
    }
}
