using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Authentication.Api.Interfaces;
using Authentication.Api.Models;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Api.Services
{
    public class AuthService : IAuthService
    {
        private IConfiguration Configuration { get; set; }
        public AuthService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        List<TokenModel> tokenModelList = new List<TokenModel>();
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public TokenModel GenerateTokenModel(string signingKey, string issuer, string audience, int accessTokenExpireMinute, int refreshTokenExpireDays, List<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(signingKey));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expiredAccessTokenTime = DateTime.Now.AddMinutes(accessTokenExpireMinute);
            var expiredRefreshTokenTime = DateTime.Now.AddDays(refreshTokenExpireDays);
            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiredAccessTokenTime,
                signingCredentials: signingCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new TokenModel
            {
                Id = Guid.NewGuid().ToString(),
                AccessToken = tokenString,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiredDateTime = expiredRefreshTokenTime
            };
        }
        public void SaveTokenModel(TokenModel tokenModel)
        {
            tokenModelList.Add(tokenModel);
        }
        public TokenModel CreateTokenByLoginModel(LoginModel loginModel)
        {
            string signingKey = Configuration["JWT:SecretKey"];
            string issuer = Configuration["JWT:ValidIssuer"];
            string audience = Configuration["JWT:ValidAudience"];
            int accessTokenValidityInMinutes = Convert.ToInt32(Configuration["JWT:AccessTokenValidityInMinutes"]);
            int refreshTokenValidityInDays = Convert.ToInt32(Configuration["JWT:RefreshTokenValidityInDays"]);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("Email", loginModel.Email));
            var tokenModel = GenerateTokenModel(signingKey, issuer, audience, accessTokenValidityInMinutes,
                refreshTokenValidityInDays, claims);
            SaveTokenModel(tokenModel);
            return tokenModel;
        }
        bool IsTokenModelExist(TokenModel tokenModel)
        {
            foreach (var token in tokenModelList)
            {
                if (token.AccessToken == tokenModel.AccessToken && token.RefreshToken == token.RefreshToken)
                {
                    return true;
                }
            }
            return false;
        }
        public TokenModel GenerateTokenModel(List<Claim> claims)
        {
            string signingKey = Configuration["JWT:SecretKey"];
            string issuer = Configuration["JWT:ValidIssuer"];
            string audience = Configuration["JWT:ValidAudience"];
            int accessTokenValidityInMinutes = Convert.ToInt32(Configuration["JWT:AccessTokenValidityInMinutes"]);
            int refreshTokenValidityInDays = Convert.ToInt32(Configuration["JWT:RefreshTokenValidityInDays"]);
            return GenerateTokenModel(signingKey, issuer, audience, accessTokenValidityInMinutes,
                refreshTokenValidityInDays, claims);
        }
        public TokenModel GetTokenModelByTokenModel(TokenModel tokenModel)
        {
            if (!IsTokenModelExist(tokenModel))
            {
                throw new Exception("Token Not found");
            }
            var claims = GetClaimsFromAccessToken(tokenModel.AccessToken);
            return GenerateTokenModel(claims);
        }
        public List<Claim> GetClaimsFromAccessToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(accessToken);
            return jwt.Claims.ToList();
        }
    }
}