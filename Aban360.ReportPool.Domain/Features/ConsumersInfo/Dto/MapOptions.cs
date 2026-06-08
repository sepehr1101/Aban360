namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record MapOptions
    {
        public const string SectionName = "Map";
        public string BaseUrl { get; set; } = default!;
        public string Statelite { get; set; } = default!;
        public string Terrain { get; set; } = default!;
        public string Road { get; set; } = default!;
        public string Google { get; set; } = default!;
        public string Hybrid { get; set; } = default!;

    }
}
