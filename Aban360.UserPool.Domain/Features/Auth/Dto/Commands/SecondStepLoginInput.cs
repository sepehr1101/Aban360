namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record SecondStepLoginInput
    {
        public Guid Id { get; set; }
        public string ConfirmCode { get; set; } = default!;
    }
}
