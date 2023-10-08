using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditUserDateAsync([FromBody] UserEditDTO userEditDTO)
        {
            await _userService.EditUserDateAsync(userEditDTO, UserId);

            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("Info")]
        public async Task<IActionResult> UserPersonalIngoAsync()
        {
            var userInfo = await _userService.GetUserPersonalInfoAsync(UserId);

            return Ok(userInfo);
        }

    }
}
