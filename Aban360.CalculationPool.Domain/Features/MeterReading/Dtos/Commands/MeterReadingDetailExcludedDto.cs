namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingDetailExcludedDto
    {
        public int Id { get; set; }
        public Guid ExcludedByUserId { get; set; }
        public DateTime ExcludedDateTime { get; set; }
        public MeterReadingDetailExcludedDto(int id, Guid excludedByUserId, DateTime excludedDateTime)
        {
            Id=id;
            ExcludedByUserId = excludedByUserId;
            ExcludedDateTime = excludedDateTime;
        }
    }
}
