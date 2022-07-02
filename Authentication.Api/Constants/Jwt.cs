namespace Authentication.Api.Constants
{
    public class Jwt
    {
        public const string SecretKey = "JWT:SecretKey";
        public const string ValidIssuer = "JWT:ValidIssuer";
        public const string ValidAudience = "JWT:ValidAudience";
        public const string AccessTokenValidityInMinutes = "JWT:AccessTokenValidityInMinutes";
        public const string RefreshTokenValidityInDays = "JWT:RefreshTokenValidityInDays";
    }
}