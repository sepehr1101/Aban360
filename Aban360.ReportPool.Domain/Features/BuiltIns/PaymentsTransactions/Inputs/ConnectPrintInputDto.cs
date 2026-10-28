namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record ConnectPrintInputDto
    {
        public string BillId { get; set; } = default!;
        public int? Why { get; set; }
        public int When { get; set; }//hour
        public int CompanyId { get; set; } 
        public Guid PersonnelId { get; set; }
        public long Amount { get; set; }
        public string? Description { get; set; }
    }
}
