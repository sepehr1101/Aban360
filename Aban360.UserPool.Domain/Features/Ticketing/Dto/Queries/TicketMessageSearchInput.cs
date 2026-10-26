namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries
{
    public class TicketMessageSearchInput
    {
        public long TicketId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
