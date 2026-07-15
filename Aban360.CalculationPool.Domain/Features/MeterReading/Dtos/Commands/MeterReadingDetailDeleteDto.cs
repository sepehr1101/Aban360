using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingDetailDeleteDto
    {
        public int Id { get; set; }
        public Guid RemovedByUserId { get; set; }
        public DateTime RemovedDateTime { get; set; }
        public short RemovedType { get; set; }
        public MeterReadingDetailDeleteDto(int id, Guid removedByUserId, DateTime removedDateTime, MeterReadingDetailRemovedType removedType)
        {
            Id = id;
            RemovedByUserId = removedByUserId;
            RemovedDateTime = removedDateTime;
            RemovedType = (short)removedType;
        }
    }
}
