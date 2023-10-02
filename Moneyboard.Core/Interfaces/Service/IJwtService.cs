using Google.Apis.Auth;
using Moneyboard.Core.DTO.UserDTO;
using Moneyboard.Core.Entities.UserEntity;
using System.Security.Claims;

namespace Moneyboard.Core.Interfaces.Service
{
    public interface IJwtService
    {
          IEnumerable<Claim> SetClaims(User user);
          string CreateToken(IEnumerable<Claim> claims);
          string CreateRefreshToken();
          IEnumerable<Claim> GetClaimsFromExpiredToken(string token);
          Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(UserExternalAuthDTO authDTO);

    }
}
