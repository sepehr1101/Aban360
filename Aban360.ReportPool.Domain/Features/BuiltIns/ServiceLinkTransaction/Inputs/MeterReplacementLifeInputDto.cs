namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs
{
    public record MeterReplacementLifeInputDto
    {
        public ICollection<int> ZoneIds { get; set; }

        public string  FromChangeDateJalali { get; set; }
        public string  ToChangeDateJalali { get; set; }

    }
}
