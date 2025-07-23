namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ServiceLinkDebtorCustomersDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public string CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public long  InstallmentDebtAmount{ get; set; }
        public long CreditorAmount { get; set; }
        public long PrincipalDebt { get; set; }
        public long TotalDebt { get; set; }

    }
}