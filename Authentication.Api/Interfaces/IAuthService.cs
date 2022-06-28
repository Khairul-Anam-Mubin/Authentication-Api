using System.Security.Claims;
using Authentication.Api.Models;

namespace Authentication.Api.Interfaces
{
    public interface IAuthService
    {
        public TokenModel GenerateTokenModel(string signingKey, string issuer, string audience, int accessTokenExpireMinute, int refreshTokenExpireDays, List<Claim> claims);
        public string GenerateRefreshToken();
        public void SaveTokenModel(TokenModel tokenModel);
        public TokenModel CreateTokenByLoginModel(LoginModel loginModel);
        public List<Claim> GetClaimsFromAccessToken(string accessToken);
        public TokenModel GetTokenModelByTokenModel(TokenModel tokenModel);
    }
}
