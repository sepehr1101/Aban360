namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record GeoOptions
    {
        public const string SectionName = "Geo";
        public string BaseUrl { get; set; }=default!;
        public string CustomerLocation { get; set; } = default!;
    }
}
