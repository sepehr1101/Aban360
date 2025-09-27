namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record ServiceLinkWaterItemGroupedInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public ICollection<int>? ZoneIds { get; set; }

        public string? FromBankId { get; set; }
        public string? ToBankId { get; set; }
    }
}
