using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Authentication.Business.Interfaces;
using Authentication.Shared.Library.Models;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Business.Services
{
    public class TokenService : ITokenService
    {
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
                Id = new Guid().ToString(),
                AccessToken = tokenString,
                RefreshToken = GenerateRefreshToken(),
                CreatedAt = DateTime.Now,
                AccessTokenExpiredDateTime = expiredAccessTokenTime,
                RefreshTokenExpiredDateTime = expiredRefreshTokenTime
            };
        }
        public void SaveTokenModel(TokenModel tokenModel)
        {
            tokenModelList.Add(tokenModel);
        }
    }
}