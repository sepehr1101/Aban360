namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionManagementByBillIdInputDto
    {
        public string BillId { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
