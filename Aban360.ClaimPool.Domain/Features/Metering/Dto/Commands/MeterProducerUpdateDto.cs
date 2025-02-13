namespace Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands
{
    public record MeterProducerUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
