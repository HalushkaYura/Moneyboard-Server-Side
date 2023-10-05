using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.DTO.UserDTO;
using Task = System.Threading.Tasks.Task;

namespace Moneyboard.Core.Interfaces.Services
{
    public interface IConfirmEmailService
    {
        Task SendConfirmMailAsync(string userId);

        Task ConfirmEmailAsync(string userId, UserConfirmEmailDTO confirmEmailDTO);

        string DecodeUnicodeBase64(string input);
    }
}
