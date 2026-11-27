namespace Aban360.MeterPool.Domain.Features.AutoReading.Dtos.Queries
{
    public record FlowMeterReportInputDto
    {
        public string FromDate { get; set; } = default!;
        public string ToDate { get; set; } = default!;
        public string BillId { get; set; } = default!;
    }
}
