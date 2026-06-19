namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkPaidDataOutputDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string BankDateJalali { get; set; }
        public string PayDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
        public long Amount { get; set; }
        public string BankCode { get; set; }
        public string? BankBranchCode { get; set; }

    }
}
