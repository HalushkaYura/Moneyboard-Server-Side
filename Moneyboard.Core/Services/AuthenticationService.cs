using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.RefreshTokenEntity;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Exeptions;
using Moneyboard.Core.Helpers.Mails;
using Moneyboard.Core.Helpers.Mails.ViewModels;
using Moneyboard.Core.Interfaces.Repository;
using Moneyboard.Core.Interfaces.Services;
using Moneyboard.Core.Resources;
using Moneyboard.Core.Roles;
using System.Net;
using System.Text;

namespace Moneyboard.Core.Services
{
    public class AuthenticationService : IAuthenticationService

    {
        protected readonly UserManager<User> _userManager;
        protected readonly SignInManager<User> _signInManager;
        protected readonly IJwtService _jwtService;
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly IRepository<RefreshToken> _refreshTokenRepository;
        protected readonly IEmailSenderService _emailSenderService;
        protected readonly IConfirmEmailService _confirmEmailService;
        protected readonly ITemplateService _templateService;
        protected readonly IOptions<ClientUrl> _clientUrl;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService jwtService,
            RoleManager<IdentityRole> roleManager,
            IRepository<RefreshToken> refreshTokenRepository,
            IEmailSenderService emailSenderService,
            IConfirmEmailService confirmEmailService,
            ITemplateService templateService,
            IOptions<ClientUrl> options,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _roleManager = roleManager;
            _refreshTokenRepository = refreshTokenRepository;
            _emailSenderService = emailSenderService;
            _confirmEmailService = confirmEmailService;
            _templateService = templateService;
            _clientUrl = options;
            IHttpContextAccessor _httpContextAccessor;
        }


        //------------------------------ LOGIN ---------------------------------------
        public async Task<UserAutorizationDTO> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new HttpException(System.Net.HttpStatusCode.Unauthorized, ErrorMessages.IncorrectLoginOrPassword);
            }

            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                return await GenerateTwoStepVerificationCode(user);
            }

            return await GenerateUserTokens(user);
        }

        //------------------------------ LOGOUT ---------------------------------------
        public async Task LogoutAsync(UserAutorizationDTO userTokensDTO)
        {
            var specification = new RefreshTokens.SearchRefreshToken(userTokensDTO.RefreshToken);
            var refeshTokenFromDb = await _refreshTokenRepository.GetFirstBySpecAsync(specification);

            if (refeshTokenFromDb == null)
            {
                return;
            }

            await _refreshTokenRepository.DeleteAsync(refeshTokenFromDb);
            await _refreshTokenRepository.SaveChangesAsync();
        }

        //------------------------------ REGISTRATION ---------------------------------------
        public async Task RegistrationAsync(User user, string password, string roleName)
        {
            user.CreateDate = DateTime.UtcNow;
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                StringBuilder errorMessage = new();
                foreach (var error in result.Errors)
                {
                    errorMessage.Append(error.Description.ToString() + " ");
                }
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, errorMessage.ToString());
            }

            var findRole = await _roleManager.FindByNameAsync(roleName);

            if (findRole == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);
        }

        //------------------------------ SentResetPassword ---------------------------------------
        public async Task SentResetPasswordTokenAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            user.UserNullChecking();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedCode = Convert.ToBase64String(Encoding.Unicode.GetBytes(token));

            await _emailSenderService.SendEmailAsync(new MailRequest()
            {
                ToEmail = user.Email,
                Subject = "Moneyboard Reset Password",
                Body = await _templateService.GetTemplateHtmlAsStringAsync("Mails/ResetPassword",
                    new UserToken() { Token = encodedCode, UserName = user.UserName, Uri = _clientUrl.Value.ApplicationUrl })
            });
        }

        //------------------------------  ResetPassword ---------------------------------------
        public async Task ResetPasswordAsync(UserChangePasswordDTO userChangePasswordDTO)
        {
            var user = await _userManager.FindByEmailAsync(userChangePasswordDTO.Email);
            user.UserNullChecking();

            var decodedCode = _confirmEmailService.DecodeUnicodeBase64(userChangePasswordDTO.Code);

            var result = await _userManager.ResetPasswordAsync(user, decodedCode, userChangePasswordDTO.NewPassword);

            if (!result.Succeeded)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.WrongResetPasswordCode);
            }
        }


        //------------------------------  ExternalLogin ---------------------------------------
        public async Task<UserAuthResponseDTO> ExternalLoginAsync(UserExternalAuthDTO authDTO)
        {
            var payload = await _jwtService.VerifyGoogleToken(authDTO);
            if (payload == null)
            {
                throw new HttpException(HttpStatusCode.BadRequest,
                    ErrorMessages.InvalidRequest);
            }

            var info = new UserLoginInfo(authDTO.Provider, payload.Subject, authDTO.Provider);

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    user = new User
                    {
                        Email = payload.Email,
                        UserName = payload.GivenName,
                        Firstname = payload.GivenName,
                        Lastname = payload.FamilyName,
                        CreateDate = DateTime.UtcNow,
                    };
                    await _userManager.CreateAsync(user);

                    var findRole = await _roleManager.FindByNameAsync(SystemRoles.User);

                    if (findRole == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SystemRoles.User));
                    }

                    await _userManager.AddToRoleAsync(user, SystemRoles.User);
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }

            var claims = _jwtService.SetClaims(user);
            var token = _jwtService.CreateToken(claims);
            var refreshToken = _jwtService.CreateRefreshToken();

            return (new UserAuthResponseDTO { Token = token, RefreshToken = refreshToken });
        }

        //------------------------------  LoginTwoStep ---------------------------------------
        public async Task<UserAutorizationDTO> LoginTwoStepAsync(UserTwoFactorDTO twoFactorDTO)
        {
            var user = await _userManager.FindByEmailAsync(twoFactorDTO.Email);
            user.UserNullChecking();

            var validVerification = await _userManager.VerifyTwoFactorTokenAsync(user, twoFactorDTO.Provider, twoFactorDTO.Token);

            if (!validVerification)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.InvalidTokenVerification);
            }

            return await GenerateUserTokens(user);
        }


        private async Task<UserAutorizationDTO> GenerateTwoStepVerificationCode(User user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);

            if (!providers.Contains("Email"))
            {
                throw new HttpException(System.Net.HttpStatusCode.Unauthorized, ErrorMessages.Invalid2StepVerification);
            }

            var twoFactorToken = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var message = new MailRequest
            {
                ToEmail = user.Email,
                Subject = "Moneyboard authentication code",
                Body = await _templateService.GetTemplateHtmlAsStringAsync("Mails/TwoFactorCode",
                    new UserToken() { Token = twoFactorToken, UserName = user.UserName, Uri = _clientUrl.Value.ApplicationUrl })
            };

            await _emailSenderService.SendEmailAsync(message);

            return new UserAutorizationDTO() { Is2StepVerificationRequired = true, Provider = "Email" };
        }


        //------------------------------  RefreshToken ---------------------------------------
        public async Task<UserAutorizationDTO> RefreshTokenAsync(UserAutorizationDTO userTokensDTO)
        {
            var specification = new RefreshTokens.SearchRefreshToken(userTokensDTO.RefreshToken);
            var refeshTokenFromDb = await _refreshTokenRepository.GetFirstBySpecAsync(specification);

            if (refeshTokenFromDb == null)
            {
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, ErrorMessages.InvalidToken);
            }

            var claims = _jwtService.GetClaimsFromExpiredToken(userTokensDTO.Token);
            var newToken = _jwtService.CreateToken(claims);
            var newRefreshToken = _jwtService.CreateRefreshToken();

            refeshTokenFromDb.Token = newRefreshToken;
            await _refreshTokenRepository.UpdateAsync(refeshTokenFromDb);
            await _refreshTokenRepository.SaveChangesAsync();

            var tokens = new UserAutorizationDTO()
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            };

            return tokens;
        }
        private async Task<UserAutorizationDTO> GenerateUserTokens(User user)
        {
            var claims = _jwtService.SetClaims(user);

            var token = _jwtService.CreateToken(claims);
            var refeshToken = await CreateRefreshToken(user);

            var tokens = new UserAutorizationDTO()
            {
                Token = token,
                RefreshToken = refeshToken
            };

            return tokens;
        }
        private async Task<string> CreateRefreshToken(User user)
        {
            var refeshToken = _jwtService.CreateRefreshToken();

            RefreshToken rt = new()
            {
                Token = (string)refeshToken,
                UserId = user.Id
            };

            await _refreshTokenRepository.AddAsync(rt);
            await _refreshTokenRepository.SaveChangesAsync();

            return (string)refeshToken;
        }









        public async Task ChangePasswordAsync(UserSetNewPasswordDTO userSetPasswordDTO, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var cheackPasword = await _userManager.CheckPasswordAsync(user, userSetPasswordDTO.CurrentPassword);
            if (!cheackPasword)
                throw new HttpException(System.Net.HttpStatusCode.BadRequest, "Unfaithful password");
            
            await _userManager.ChangePasswordAsync(user, userSetPasswordDTO.CurrentPassword, userSetPasswordDTO.NewPassword);

        }


    }
    
}
