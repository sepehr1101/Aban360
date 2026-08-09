namespace Aban360.CalculationPool.Domain.Features.CollectBills.Inputs
{
    public record CollectBillsInputDto<T>
    {
        public T Parameters { get; set; }
        public CollectBillsIdentityInputDto Identity { get; set; }
        public CollectBillsInputDto(T parameters, CollectBillsIdentityInputDto identity)
        {
            Parameters = parameters;
            Identity = identity;
        }
        public CollectBillsInputDto()
        {
        }
    }
}
