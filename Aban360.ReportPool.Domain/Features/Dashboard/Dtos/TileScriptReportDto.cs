namespace Aban360.ReportPool.Domain.Features.Dashboard.Dtos
{
    public record TileScriptReportDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public TileScriptReportDto(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
