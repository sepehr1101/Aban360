namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record MeterDuplicateChangeWithCustomerDetailDataOutputDto
    {
        public int MeterNumber { get; set; }
        public string ChangeDateJalali { get; set; }
        public string RegisterDateJalali { get; set; }
        public string ChangeCauseTitle { get; set; }
        public string BodySerial { get; set; }
    }
}