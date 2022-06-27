﻿namespace Authentication.Shared.Library.Models
{
    public class TokenModel
    {
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime AccessTokenExpiredDateTime { get; set; }
        public DateTime RefreshTokenExpiredDateTime { get; set; }
    }
}