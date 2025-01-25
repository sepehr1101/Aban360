namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public class FirstStepLoginInput
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ClientDateTime { get; set; }
        public string? AppVersion { get; set; }
        public string CaptchaToken { get; set; } = null!;
        public string CaptchaInputText { get; set; } = null!;
    }
}