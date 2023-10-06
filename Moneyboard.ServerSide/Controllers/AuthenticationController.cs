using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Interfaces.Services;
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
                ImageUrl = registrationDTO.ImageUrl
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

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] UserAutorizationDTO userTokensDTO)
        {
            await _authenticationService.LogoutAsync(userTokensDTO);

            return NoContent();
        } 


        [HttpGet]
        [Route("password/{email}")]
        public async Task<IActionResult> SentResetPasswordTokenAsync(string email)
        {
            await _authenticationService.SentResetPasswordTokenAsync(email);

            return Ok();
        }

        [HttpGet("emails")]
        public async Task<IActionResult> GetEmails([FromQuery] string email)
        {
            bool emails = await _authenticationService.GetAllUserEmailsAsync(email);

            return Ok(emails);
        }

        [HttpPost]
        [Route("login-two-step")]
        public async Task<IActionResult> LoginTwoStepAsync([FromBody] UserTwoFactorDTO twoFactorDTO)
        {
            var tokens = await _authenticationService.LoginTwoStepAsync(twoFactorDTO);

            return Ok(tokens);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] UserAutorizationDTO userTokensDTO)
        {
            var tokens = await _authenticationService.RefreshTokenAsync(userTokensDTO);

            return Ok(tokens);
        }

        [HttpPut]
        [Route("password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] UserChangePasswordDTO userChangePasswordDTO)
        {
            await _authenticationService.ResetPasswordAsync(userChangePasswordDTO);

            return Ok();
        }

        [HttpPost]
        [Route("signin-google")]
        public async Task<IActionResult> ExternalLoginAsync([FromBody] UserExternalAuthDTO authDTO)
        {
            var result = await _authenticationService.ExternalLoginAsync(authDTO);
            return Ok(result);
        }
    }
}
