namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record BankGroupedInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public int? FromBankId { get; set; }
        public int? ToBankId { get; set; }
    }
}
