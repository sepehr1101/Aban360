using Aban360.CalculationPool.Domain.Constants;
using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.CollectBills.Outputs
{
    public record CollectBillsDetailWithLastStepDataOutputDto
    {
        public int FirstId { get; set; }
        public int LastId { get; set; }
        public Guid GroupingId { get; set; }
        public CollectBillStepEnum FirstStepId { get; set; }
        public CollectBillStepEnum LastStepId { get; set; }
        public string? FileName { get; set; }
        public DateTime FirstStepInsertDateTime { get; set; }
        public string FirsrStepInsertDateTimeJalali { get { return FirstStepInsertDateTime.ToShortPersianDateTimeString(); } }
        public DateTime LastStepInserteDateTime { get; set; }
        public string LastStepInserteDateTimeJalali { get { return LastStepInserteDateTime.ToShortPersianDateTimeString(); } }
        public DateTime? LastStepFinishedDateTime { get; set; }
        public string? LastStepFinishedDateTimeJalali { get { return LastStepFinishedDateTime?.ToShortPersianDateTimeString() ?? string.Empty; } }
        public string? LastStepDescription { get; set; }
    }
}