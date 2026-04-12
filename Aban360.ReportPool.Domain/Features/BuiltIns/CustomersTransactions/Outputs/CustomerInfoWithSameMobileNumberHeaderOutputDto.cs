namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs
{
    public record CustomerInfoWithSameMobileNumberHeaderOutputDto
    {
        public string MobileNumber { get; set; }
        public string  ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
    }
}
