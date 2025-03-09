namespace Aban360.MeterPool.Domain.Features.Management.Dtos.Commands
{
    public record ReadingPeriodTypeCreateDto
    {
        public string Title { get; set; } = null!;
        public short Days { get; set; }
        public short ClientOrder { get; set; }
        public bool IsEnabled { get; set; }
        public short HeadquartersId { get; set; }
    }
}
