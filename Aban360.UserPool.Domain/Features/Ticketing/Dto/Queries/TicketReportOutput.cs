namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries
{
    public class TicketReportOutput
    {
        public int TicketCount { get; set; }
        public int RatingCount { get; set; }
        public double? AverageScore { get; set; }
        public ICollection<TicketStatusCount> StatusCounts { get; set; } = new List<TicketStatusCount>();
    }
}
