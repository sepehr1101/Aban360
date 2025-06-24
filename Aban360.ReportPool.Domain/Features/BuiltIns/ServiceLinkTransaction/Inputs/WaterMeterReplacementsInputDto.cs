namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record WaterMeterReplacementsInputDto
    {
        public bool BasedOnChange { get; set; }
        public string FromDateJalali { get; set; } = null!;
        public string ToDateJalali { get; set; } = null!;
    }
}
