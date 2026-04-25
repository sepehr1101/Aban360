namespace Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs
{
    public record InstallationPrintInputDto
    {
        public string BillId { get; set; }
        public string  Base64Image { get; set; }
    }
}
