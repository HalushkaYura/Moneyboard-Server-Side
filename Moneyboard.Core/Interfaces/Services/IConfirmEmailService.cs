using Moneyboard.Core.DTO.UserDTO;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IConfirmEmailService
    {
        Task SendConfirmMailAsync(string userId);

        Task ConfirmEmailAsync(string userId, UserConfirmEmailDTO confirmEmailDTO);

        string DecodeUnicodeBase64(string input);
    }
}
