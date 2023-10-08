using Microsoft.AspNetCore.Http;

namespace Moneyboard.Core.DTO.UserDTO
{
    public class UserUploadImageDTO
    {
        public IFormFile Image { get; set; }
    }
}
