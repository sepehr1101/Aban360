namespace Aban360.ReportPool.Domain.Features.Dashboard.Dtos
{
    public record TileScriptContentReportInputDto
    {
        public IEnumerable<int> ZoneIds{ get; set; }
        public string CurrentDateJalali { get; set; }
        public string? FromDateJalali { get; set; }
        public string? ToDateJalali { get; set; }
        public TileScriptContentReportInputDto(IEnumerable<int> zoneIds,string currentDateJalali,string? fromDateJalali,string? toDateJalali)
        {
            ZoneIds = zoneIds;  
            CurrentDateJalali = currentDateJalali;  
            FromDateJalali= fromDateJalali;
            ToDateJalali= toDateJalali;
        }
    }
}
