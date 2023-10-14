using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Roles;

namespace Moneyboard.ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly Core.Interfaces.Services.IAuthenticationService _authenticationService;
        public AuthenticationController(Core.Interfaces.Services.IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> RegistrationAsync([FromBody] UserRegistrationDTO registrationDTO)
        {
            var user = new User()
            {
                UserName = registrationDTO.Email,
                Lastname = registrationDTO.Lastname,
                Firstname = registrationDTO.Firstname,
                Email = registrationDTO.Email,
                BirthDate = registrationDTO.BirthDay,
                CardNumber = registrationDTO.CardNumber,
                ImageUrl = "https://media.istockphoto.com/id/1300845620/uk/%D0%B2%D0%B5%D0%BA%D1%82%D0%BE%D1%80%D0%BD%D1%96-%D0%B7%D0%BE%D0%B1%D1%80%D0%B0%D0%B6%D0%B5%D0%BD%D0%BD%D1%8F/%D0%BF%D1%96%D0%BA%D1%82%D0%BE%D0%B3%D1%80%D0%B0%D0%BC%D0%B0-%D0%BA%D0%BE%D1%80%D0%B8%D1%81%D1%82%D1%83%D0%B2%D0%B0%D1%87%D0%B0-%D0%BF%D0%BB%D0%BE%D1%81%D0%BA%D0%B0-%D1%96%D0%B7%D0%BE%D0%BB%D1%8C%D0%BE%D0%B2%D0%B0%D0%BD%D0%B0-%D0%BD%D0%B0-%D0%B1%D1%96%D0%BB%D0%BE%D0%BC%D1%83-%D1%82%D0%BB%D1%96-%D1%81%D0%B8%D0%BC%D0%B2%D0%BE%D0%BB-%D0%BA%D0%BE%D1%80%D0%B8%D1%81%D1%82%D1%83%D0%B2%D0%B0%D1%87%D0%B0.jpg?s=612x612&w=0&k=20&c=0lzdKCv-3C6nY9LOuP8Embv_wKnmUOHI1p71OyKKL9Y="
            };

            await _authenticationService.RegistrationAsync(user, registrationDTO.Password, SystemRoles.User);

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDTO userLoginDTO)
        {
            var tokens = await _authenticationService.LoginAsync(userLoginDTO.Email, userLoginDTO.Password);

            return Ok(tokens);
        }
        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] UserAutorizationDTO userTokensDTO)
        {
            await _authenticationService.LogoutAsync(userTokensDTO);

            return NoContent();
        }


        [Authorize]
        [HttpGet]
        [Route("password/{email}")]
        public async Task<IActionResult> SentResetPasswordTokenAsync(string email)
        {
            await _authenticationService.SentResetPasswordTokenAsync(email);

            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("emails")]
        public async Task<IActionResult> GetEmails([FromQuery] string email)
        {
            User user = await _authenticationService.GetAllUserEmailsAsync(email);

            return Ok(user);
        }

        [Authorize]
        [HttpPost]
        [Route("login-two-step")]
        public async Task<IActionResult> LoginTwoStepAsync([FromBody] UserTwoFactorDTO twoFactorDTO)
        {
            var tokens = await _authenticationService.LoginTwoStepAsync(twoFactorDTO);

            return Ok(tokens);
        }
        [Authorize]
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] UserAutorizationDTO userTokensDTO)
        {
            var tokens = await _authenticationService.RefreshTokenAsync(userTokensDTO);

            return Ok(tokens);
        }
        [Authorize]
        [HttpPut]
        [Route("password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] UserChangePasswordDTO userChangePasswordDTO)
        {
            await _authenticationService.ResetPasswordAsync(userChangePasswordDTO);

            return Ok();
        }
        [Authorize]
        [HttpPost]
        [Route("signin-google")]
        public async Task<IActionResult> ExternalLoginAsync([FromBody] UserExternalAuthDTO authDTO)
        {
            var result = await _authenticationService.ExternalLoginAsync(authDTO);
            return Ok(result);
        }



    }
}
