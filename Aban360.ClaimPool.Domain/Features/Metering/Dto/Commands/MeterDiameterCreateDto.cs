namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record MeterDiameterCreateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
