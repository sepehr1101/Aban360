namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record ChangePasswordInput
    {
        public string OldPassword { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string PasswordConfirm { get; set; } = default!;
    }
}
