namespace Aban360.CalculationPool.Domain.Features.CollectBills.Outputs
{
    public record CollectBillsOutputDto<T>
    {
        public bool IsSuccess { get; set; }
        public int Code { get; set; }
        public T? Result { get; set; }
        public string Message { get; set; }
        public IEnumerable<CollectBillsErrorOutputDto>? Errors{ get; set; }
    }
}
