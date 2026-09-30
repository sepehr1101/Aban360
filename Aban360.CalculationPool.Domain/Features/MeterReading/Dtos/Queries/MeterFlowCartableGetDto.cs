using Aban360.CalculationPool.Domain.Constants;
using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterFlowCartableGetDto
    {
        public short Id { get; set; }
        public MeterFlowStepEnum MeterFlowStepId { get; set; }
        public string StepTitle { get; set; }
        public short FirstFlowId { get; set; }
        public string FileName { get; set; }
        public int ZoneId { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }
        public int PrimaryCount { get; set; }
        public string ZoneTitle { get; set; }
        public DateTime InsertDateTime { get; set; }
        public string InsertDateTimeJalali { get { return InsertDateTime.ToShortPersianDateTimeString(); } }
        public Guid InsertByUserId { get; set; }
        public string? Description { get; set; }

    }
}
