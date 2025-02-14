namespace Aban360.ClaimPool.Domain.Features.Registration.Dto.Command
{
    public record UseStateUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
