namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries
{
    public class TicketMySearchInput
    {
        public short? StatusId { get; set; }
        public string? SearchText { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDateExclusive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
