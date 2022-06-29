using System.Security.Claims;
using Authentication.Api.Models;
namespace Authentication.Api.Interfaces
{
    public interface IAuthService
    {
        public string GenerateAccessToken(List<Claim> claims);
        public string GenerateRefreshToken();
        public DateTime GetRefreshTokenExpiredDateTime();
        public TokenModel CreateTokenModel(UserModel userModel);
        public TokenModel GetTokenModel(LoginModel loginModel);
        public TokenModel GetTokenModel(TokenModel tokenModel);
        bool IsExpiredRefreshToken(TokenModel tokenModel);
        List<Claim> GetClaimsFromAccessToken(string accessToken);

        // DB Call methods
        TokenModel GetTokenModelByAccessToken(string accessToken);
        public void SaveTokenModel(TokenModel tokenModel);
        public TokenModel UpdateTokenModel(TokenModel tokenModel);
    }
}