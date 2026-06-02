namespace Aban360.ReportPool.Domain.Features.Request.Inputs
{
    public record ReceivedSmsInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
    }
}
