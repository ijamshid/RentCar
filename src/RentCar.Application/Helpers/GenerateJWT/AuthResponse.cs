namespace RentCar.Application.Helpers.GenerateJWT
{
    public class AuthResponse
    {
        public string Email { get; set; } = default!;
        public string Fullname { get; set; } = default!;
        public string Token { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
