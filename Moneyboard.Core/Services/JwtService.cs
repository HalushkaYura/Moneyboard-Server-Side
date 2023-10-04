using Google.Apis.Auth;
using Jose;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Helpers;
using ServiceStack;
using ServiceStack.Host;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Moneyboard.Core.Interfaces.Services;

namespace Maneyboard.Core.Services
{
    public class JwtService : IJwtService
    {
        private readonly Microsoft.Extensions.Options.IOptions<Moneyboard.Core.Helpers.JwtOptions> jwtOptions;
        private readonly UserManager<User> userManager;
        protected readonly IConfigurationSection _googleSettings;
        private readonly IConfiguration _configuration;

        public JwtService(Microsoft.Extensions.Options.IOptions<Moneyboard.Core.Helpers.JwtOptions> jwtOptions, UserManager<User> userManager,
            IConfiguration configuration)
        {
            this.jwtOptions = jwtOptions;
            this.userManager = userManager;
            _configuration = configuration;
            _googleSettings = _configuration.GetSection("GoogleAuthSettings");
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public string CreateToken(IEnumerable<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Value.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(jwtOptions.Value.LifeTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IEnumerable<Claim> GetClaimsFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Value.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key)),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken;

            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new HttpException((int)System.Net.HttpStatusCode.BadRequest, ErrorMessages.InvalidAccessToken);

            return jwtSecurityToken.Claims;
        }

        public IEnumerable<Claim> SetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email),
            };

            var roles = userManager.GetRolesAsync(user).Result;
            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            return claims;
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(UserExternalAuthDTO authDTO)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _googleSettings.GetSection("clientId").Value }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(authDTO.IdToken, settings);
            return payload;
        }
    }
}
