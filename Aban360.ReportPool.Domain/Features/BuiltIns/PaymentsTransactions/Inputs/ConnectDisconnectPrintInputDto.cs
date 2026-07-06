namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record ConnectDisconnectPrintInputDto
    {
        public string BillId { get; set; } = default!;
        public int? Why { get; set; }
        public int When { get; set; }//hour
        public int CompanyId { get; set; } 
        public Guid PersonnelId { get; set; } 
        public string? Description { get; set; }
    }
}
