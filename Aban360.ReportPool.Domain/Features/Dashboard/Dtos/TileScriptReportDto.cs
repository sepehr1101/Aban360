namespace Aban360.ReportPool.Domain.Features.Dashboard.Dtos
{
    public record TileScriptReportDto
    {
        public string Key { get; set; }
        public long Value { get; set; }
        public TileScriptReportDto(string key, long value)
        {
            Key = key;
            Value = value;
        }
        public TileScriptReportDto()
        {
        }
    }
}
