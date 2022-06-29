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
        private IConfiguration Configuration { get; }
        private IUserService UserService { get; }

        public AuthService(IConfiguration configuration, IUserService userService)
        {
            Configuration = configuration;
            UserService = userService;
        }

        List<TokenModel> tokenModelList = new List<TokenModel>();

        public string GenerateAccessToken(List<Claim> claims)
        {
            // JWT Config
            string signingKey = Configuration["JWT:SecretKey"];
            string validIssuer = Configuration["JWT:ValidIssuer"];
            string audience = Configuration["JWT:ValidAudience"];
            int accessTokenValidityInMinutes = Convert.ToInt32(Configuration["JWT:AccessTokenValidityInMinutes"]);

            // JWT Creation
            var secretKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(signingKey));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expiredAccessTokenDateTime = DateTime.Now.AddMinutes(accessTokenValidityInMinutes);
            var tokenOptions = new JwtSecurityToken(
                issuer: validIssuer,
                audience: audience,
                claims: claims,
                expires: expiredAccessTokenDateTime,
                signingCredentials: signingCredentials
            );
            string accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return accessToken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public DateTime GetRefreshTokenExpiredDateTime()
        {
            var refreshTokenValidityInDays = Convert.ToInt32(Configuration["JWT:RefreshTokenValidityInDays"]);
            return DateTime.Now.AddDays(refreshTokenValidityInDays);
        }

        public TokenModel CreateTokenModel(UserModel userModel)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("UserRole", userModel.UserRole));
            claims.Add(new Claim("Email", userModel.Email));
            string id = Guid.NewGuid().ToString();
            string accessToken = GenerateAccessToken(claims);
            string refreshToken = GenerateRefreshToken();
            var refreshTokeExpiredDateTime = GetRefreshTokenExpiredDateTime();
            return new TokenModel
            {
                Id = id,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiredDateTime = refreshTokeExpiredDateTime
            };
        }

        public TokenModel GetTokenModel(LoginModel loginModel)
        {
            if (!UserService.IsUserEmailAndPasswordExist(loginModel))
                throw new Exception("User Credential is not valid.");
            UserModel userModel = UserService.GetUserByEmail(loginModel.Email);
            TokenModel tokenModel = CreateTokenModel(userModel);
            SaveTokenModel(tokenModel);
            return tokenModel;
        }

        public TokenModel GetTokenModel(TokenModel tokenModel)
        {
            var tokenModelFromContext = GetTokenModelByAccessToken(tokenModel.AccessToken);
            if (tokenModelFromContext == null) throw new Exception("Access Token not valid.");
            if (tokenModel.RefreshToken != tokenModelFromContext.RefreshToken)
                throw new Exception("Refresh Token not valid");
            if (IsExpiredRefreshToken(tokenModelFromContext)) throw new Exception("Refresh Token Expired");

            // TokenModel is valid
            var claims = GetClaimsFromAccessToken(tokenModelFromContext.AccessToken);
            tokenModelFromContext.AccessToken = GenerateAccessToken(claims);
            tokenModelFromContext.RefreshToken = GenerateRefreshToken();
            tokenModelFromContext.RefreshTokenExpiredDateTime = GetRefreshTokenExpiredDateTime();
            return UpdateTokenModel(tokenModelFromContext);
        }

        public bool IsExpiredRefreshToken(TokenModel tokenModel)
        {
            var currentDateTime = DateTime.Now;
            int compare = DateTime.Compare(currentDateTime, tokenModel.RefreshTokenExpiredDateTime);
            if (compare > 0) return true;
            return false;
        }

        public List<Claim> GetClaimsFromAccessToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(accessToken);
            return jwt.Claims.ToList();
        }

        public TokenModel GetTokenModelByAccessToken(string accessToken)
        {
            foreach (var tokenModel in tokenModelList)
            {
                if (tokenModel.AccessToken == accessToken)
                {
                    return tokenModel;
                }
            }

            return null;
        }
        public void SaveTokenModel(TokenModel tokenModel)
        {
            tokenModelList.Add(tokenModel);
        }
        public TokenModel UpdateTokenModel(TokenModel tokenModel)
        {
            for (int i = 0; i < tokenModelList.Count; i++)
            {
                if (tokenModelList[i].Id == tokenModel.Id)
                {
                    tokenModelList[i] = tokenModel;
                    break;
                }
            }
            return tokenModel;
        }
    }
}