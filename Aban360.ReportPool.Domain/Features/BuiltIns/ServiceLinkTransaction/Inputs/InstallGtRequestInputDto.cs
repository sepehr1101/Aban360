namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record InstallGtRequestInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public int Distance { get; set; }

    }
}
