namespace Aban360.Common.Authentication
{
    public sealed class EsbAuthenticationOptions
    {
        public const string SectionName = "EsbAuthentication";

        public string BaseUrl { get; set; } = default!;
        public string TokenEndpoint { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
