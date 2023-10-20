using Microsoft.AspNetCore.Http;

namespace Moneyboard.Core.DTO.UserDTO
{
    public class UserImageUploadDTO
    {
        public IFormFile Image { get; set; }
    }
}
