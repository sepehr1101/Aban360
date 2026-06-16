namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record ConnectDisconnectGetWithConditionDto
    {
        public string BillId { get; set; }
        public int TypeId { get; set; }
        public bool HasResult { get; set; }
        public bool IsRemoved { get; set; }
        public ConnectDisconnectGetWithConditionDto(string billId, int typeId, bool hasResult, bool isRemoved)
        {
            BillId = billId;
            TypeId = typeId;
            HasResult = hasResult;
            IsRemoved = isRemoved;
        }
        public ConnectDisconnectGetWithConditionDto()
        {
        }
    }
}
