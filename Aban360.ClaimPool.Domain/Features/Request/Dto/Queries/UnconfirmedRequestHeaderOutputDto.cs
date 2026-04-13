namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record UnconfirmedRequestHeaderOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int RecordCount { get; set; }
        public string Title { get; set; }
        public string ReportDateJalali { get; set; }
        public long Amount { get; set; }
    }
}
