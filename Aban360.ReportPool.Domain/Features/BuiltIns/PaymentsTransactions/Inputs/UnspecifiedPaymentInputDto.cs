namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record UnspecifiedPaymentInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public long? FromAmount { get; set; }
        public long? ToAmount { get; set; }

        public string FromBankId { get; set; }
        public string ToBankId { get; set; }

    }
}

