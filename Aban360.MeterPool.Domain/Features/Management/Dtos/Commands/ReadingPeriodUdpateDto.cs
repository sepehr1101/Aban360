namespace Aban360.MeterPool.Domain.Features.Management.Dtos.Commands
{
    public record ReadingPeriodUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short ReadingPeriodTypeId { get; set; }
        public short ClientOrder { get; set; }
    }
}
