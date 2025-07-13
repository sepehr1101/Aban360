namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerInfoByBillIdOutputDto
    {
        public string BillId { get; set; } = default!;
        public int  CustomerNumber { get; set; }
        public string ReadingNumber { get; set; } = default!;
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; } = default!;
        public int? OldCustomerNumber { get; set; }
        public string? OldBillId { get; set; }
    }
}
