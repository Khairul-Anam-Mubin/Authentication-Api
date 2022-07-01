using Authentication.Api.Interfaces;

namespace Authentication.Api.Models
{
    public class TokenModel : IRepositoryItem
    {
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiredDateTime { get; set; }
    }
}