namespace Blessings.Services.Options
{
    public class JwtTokenOptions
    {
        public const string Section = "JwtToken";

        public byte[] Secret { get; set; }
        public int AccessTokenDurationInMinutes { get; set; }
        public int AccessTokenDurationInMinutesRememberMe { get; set; }
    }
}
