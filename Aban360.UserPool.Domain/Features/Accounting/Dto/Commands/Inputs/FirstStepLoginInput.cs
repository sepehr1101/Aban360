namespace Aban360.UserPool.Domain.Features.Accounting.Dto.Commands.Inputs
{
    public class FirstStepLoginInput
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string DeviceSerial { get; set; } = null!;
        public string AppVersion { get; set; } = null!;
        public string ClientDateTime { get; set; } = null!;
        public string Aban360CaptchaText { get; set; } = null!;
        public string Aban360CaptchaInputText { get; set; } = null!;
        public string Aban360CaptchaToken { get; set; } = null!;
    }
}
