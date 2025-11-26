namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingDetailDeleteDto
    {
        public int Id { get; set; }
        public Guid RemovedByUserId { get; set; }
        public DateTime RemovedDateTime { get; set; }
        public MeterReadingDetailDeleteDto(int id, Guid removedByUserId, DateTime removedDateTime)
        {
            Id=id;
            RemovedByUserId=removedByUserId;
            RemovedDateTime=removedDateTime;
        }
    }
}
