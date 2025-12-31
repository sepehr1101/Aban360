namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record MeterDuplicateChangeWithCustomerInputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }

        public string FromDateJalali { get; set; } = default!;
        public string ToDateJalali { get; set; } = default!;

        public bool IsRegisterDate { get; set; }
    }
}
