using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.UserEntity;
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
        ////////////////////////////////////////////////////////////////////////////////


        [Authorize]
        [HttpPut]
        [Route("upload-foto")]
        public async Task<IActionResult> UploadUserImage([FromForm] UserImageUploadDTO imageDTO)
        {
            await _userService.UploadAvatar(imageDTO, UserId);
            return Ok("Image uploaded successfully");

        }
        [Authorize]
        [HttpGet]
        [Route("get-image/{userId}")]
        public async Task<IActionResult> GetUserImage(string userId)
        {
            var imageUrl = await _userService.GetUserImageAsync(userId);
            return Ok(imageUrl);
        }

    }
}
