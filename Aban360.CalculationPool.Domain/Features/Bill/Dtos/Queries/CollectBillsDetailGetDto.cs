using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record CollectBillsDetailGetDto
    {
        public int Id { get; set; }
        public Guid GroupingId { get; set; }
        public int StepId { get; set; }
        public string StepTitle { get; set; }
        public int StepOrder { get; set; }

        public DateTime InsertDateTime { get; set; }
        public string InsertDateJalali { get { return InsertDateTime.ToShortPersianDateString(); } }

        public DateTime FinishDateTime { get; set; }
        public string FinishDateJalali { get { return FinishDateTime.ToShortPersianDateString(); } }
    }
}
