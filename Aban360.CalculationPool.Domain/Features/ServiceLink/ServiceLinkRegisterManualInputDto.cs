namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record ServiceLinkRegisterManualInputDto
    {
        public int ZoneId { get; set; }
        public string PayDateJalali { get; set; } = default!;//تاریخ عملکرد
        public string BankDateJalali { get; set; } = default!;//تاریخ بانک

        public int CustomerNumber { get; set; }
        public int BankCode { get; set; }
        public string? BankBranchCode { get; set; }
        public long Amount { get; set; }
        public string? ChequeOrMinutesNumber { get; set; }
    }
}