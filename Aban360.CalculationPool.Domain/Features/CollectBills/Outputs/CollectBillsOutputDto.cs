namespace Aban360.CalculationPool.Domain.Features.CollectBills.Outputs
{
    public record CollectBillsOutputDto<T>
    {
        public T Parameters { get; set; }
        public CollectBillsStatusOutputDto Status { get; set; }
    }
}
