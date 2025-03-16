namespace Aban360.MeterPool.Domain.Features.Management.Dtos.Queries
{
    public record ReadingPeriodGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short ReadingPeriodTypeId { get; set; }
        public string ReadingPeriodTypeTitle { get; set; }
        public int ClientOrder { get; set; }
    }
}
