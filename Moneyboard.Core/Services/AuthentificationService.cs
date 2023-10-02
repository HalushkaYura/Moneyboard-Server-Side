using Maneyboard.Core.Resources;
using Microsoft.AspNetCore.Identity;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.RefreshTokenEntity;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Exceptions;
using Moneyboard.Core.Interfaces.Service;
using Nest;
using Moneyboard.Core.Interfaces.Repositor;


namespace Moneyboard.Core.Services
{
    public class AuthentificationService : IAuthentificationService

    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;
        //protected readonly IRepository<RefreshToken> _refreshTokenRepository;
        protected readonly IRepositor<RefreshToken> _refreshTokenRepository;

        // private readonly IConfiguration _configuration;

        public AuthentificationService(UserManager<User> userManager, IRepositor<RefreshToken> refreshTokenRepository, IJwtService jwtService, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;

        }

        public async Task<UserAutorizationDTO> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new HttpException(System.Net.HttpStatusCode.Unauthorized, ErrorMessages.IncorrectLoginOrPassword);
            }

            return await GenerateUserTokens(user);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        //REGISTRATION 

        public Task RegistrationAsync(User user, string password)
        {
            throw new NotImplementedException();
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







        /*public async Task RegistrationAsync(User user, string password)
        {
            user.CreateDate = DateTime.Now;
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

        }
        public async Task<UserAutorizationDTO> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                //ErrorMessages.IncorrectLoginOrPassword 
                throw new HttpException(System.Net.HttpStatusCode.Unauthorized, "Incorrect login or password");
            }
            return await GenerateUserTokens(user);
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
                Token = refeshToken,
                UserId = user.Id
            };

            await _refreshTokenRepository.AddAsync(rt);
            await _refreshTokenRepository.SaveChangesAsync();

            return refeshToken;
        }


        public Task LogoutAsync(UserAutorizationDTO userTokensDTO)
        {
            var refeshTokenFromDb = await _refreshTokenRepository.GetFirstBySpecAsync(specification);

            if (refeshTokenFromDb == null)
            {
                return;
            }

            await _refreshTokenRepository.DeleteAsync(refeshTokenFromDb);
            await _refreshTokenRepository.SaveChangesAsync();
        }*/




    }
}
