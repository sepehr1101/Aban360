namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerInfoByBillIdOutputDto
    {
        public string  CustomerNumber { get; set; }
        public string ReadingNumber { get; set; }
    }
}
