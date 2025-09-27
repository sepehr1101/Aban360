namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record ServiceLinkDebtorCustomersHeaderOutputDto
    {
        public long FromAmount { get; set; }
        public long ToAmount{ get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }

        public long SumInstallmentDebtAmout { get; set; }
        public long SumCreditAmount { get; set; }
        public long SumPrincipalDebt { get; set; }
        public long SumTotalDebt { get; set; }
    }
}
