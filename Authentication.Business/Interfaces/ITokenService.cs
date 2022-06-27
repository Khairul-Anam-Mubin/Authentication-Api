using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Authentication.Shared.Library.Models;


namespace Authentication.Business.Interfaces
{
    public interface ITokenService
    {
        public TokenModel GenerateTokenModel(string signingKey, string issuer, string audience, int accessTokenExpireMinute, int refreshTokenExpireDays, List<Claim> claims);
        public string GenerateRefreshToken();
        public void SaveTokenModel(TokenModel tokenModel);
    }
}
