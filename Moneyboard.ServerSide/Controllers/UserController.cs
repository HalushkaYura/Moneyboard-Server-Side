using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Interfaces.Services;
using System.Security.Claims;

namespace Moneyboard.ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        [Route("image")]
        public async Task<IActionResult> GetUserImage()
        {
            // Отримати фотографію користувача за ідентифікатором користувача
            var image = await _userService.GetUserImageAsync(UserId);

            if (image == null)
            {
                return NotFound(); // Якщо фотографія не знайдена
            }

            return File(image.Content, image.ContentType); // Повернути фотографію у відповіді
        }


        [HttpPut]
        [Authorize]
        [Route("image")]
        public async Task<IActionResult> UpdateImageAsync([FromForm] UserImageUploadDTO uploadImage)
        {
            await _userService.UpdateUserImageAsync(UserId, uploadImage.Image);
            return Ok("Фотографію користувача оновлено");
        }


       

        [HttpGet]
        [Authorize]
        [Route("image/user/{userId}")]
        public async Task<FileResult> GetImageAsync(string userId)
        {
            var file = await _userService.GetUserImageAsync(userId);

            return File(file.Content, file.ContentType, file.Name);
        }

        [Authorize]
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditUserDateAsync([FromBody] UserEditDTO userEditDTO)
        {
            await _userService.EditUserDateAsync(userEditDTO, UserId);

            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> UserPersonalIngoAsync()
        {
            var userInfo = await _userService.UserInfoAsync(UserId);

            return Ok(userInfo);
        }

    }
}
