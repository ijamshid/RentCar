﻿namespace RentCar.Application.Helpers.GenerateJWT
{
    public class JwtOption
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int ExpirationInSeconds { get; set; }
    }
}