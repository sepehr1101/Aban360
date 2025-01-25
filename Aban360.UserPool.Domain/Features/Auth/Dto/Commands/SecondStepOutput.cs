namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record SecondStepOutput
    {
        public string AccessToken { get; } = default!;
        public string RefreshToken { get; } = default!;
        public SecondStepOutput(string access, string refresh)
        {
            AccessToken = access;
            RefreshToken = refresh;
        }
    }
}
