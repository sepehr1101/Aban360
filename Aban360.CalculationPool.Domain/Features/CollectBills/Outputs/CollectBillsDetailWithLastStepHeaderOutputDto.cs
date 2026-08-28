using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.CollectBills.Outputs
{
    public record CollectBillsDetailWithLastStepHeaderOutputDto
    {
        public string ReprotDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title{ get; set; }
        public int RecordCount { get; set; }
    }
}