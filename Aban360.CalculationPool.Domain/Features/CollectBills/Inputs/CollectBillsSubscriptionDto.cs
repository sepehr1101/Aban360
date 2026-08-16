using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsSubscriptionDto
    {
        public string Bill_Identifier { get; set; }
        public string Pay_Identifier { get; set; }
        public string? Payment_Date { get; set; }
        public CollectBillsPaymentStatusEnum Payment_Status { get; set; }
    }

}