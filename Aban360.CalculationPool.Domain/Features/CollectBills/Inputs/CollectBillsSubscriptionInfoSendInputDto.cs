namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsSubscriptionInfoSendInputDto
    {
        public string Origin { get; set; }
        public IEnumerable<CollectBillsSubscriptionDto> Data { get; set; }

    }
}