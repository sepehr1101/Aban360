namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ConnectDisconnectHeaderOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public string CurrentDateJalali { get; set; }
        public int ConnectCount { get; set; }
        public int DisconnectCount { get; set; }
        public int Count { get; set; }
        public int RecordCount { get; set; }
        public string Title { get; set; }
    }
}
