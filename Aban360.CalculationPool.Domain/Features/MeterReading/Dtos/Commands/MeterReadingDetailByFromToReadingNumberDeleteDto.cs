using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands
{
    public record MeterReadingDetailByFromToReadingNumberDeleteDto
    {
        public int FlowImportedId { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public Guid RemovedByUserId { get; set; }
        public DateTime RemovedDateTime { get; set; }
        public short RemovedType { get; set; }
        public MeterReadingDetailByFromToReadingNumberDeleteDto(int flowImportedId, string fromReadingNumber, string toReadingNumber, Guid removedByUserId, DateTime removedDateTime, MeterReadingDetailRemovedType removedType)
        {
            FlowImportedId = flowImportedId;
            FromReadingNumber = fromReadingNumber;
            ToReadingNumber = toReadingNumber;
            RemovedByUserId = removedByUserId;
            RemovedDateTime = removedDateTime;
            RemovedType = (short)removedType;
        }
        public MeterReadingDetailByFromToReadingNumberDeleteDto()
        {
        }
    }
}
