using Authentication.Api.Interfaces;

namespace Authentication.Api.Models
{
    public class TokenModel : IRepositoryItem
    {
        public string Id { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiredDateTime { get; set; }
    }
}