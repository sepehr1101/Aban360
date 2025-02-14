namespace Aban360.ClaimPool.Domain.Features.Registration.Dto.Command
{
    public record UseStateCreateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
