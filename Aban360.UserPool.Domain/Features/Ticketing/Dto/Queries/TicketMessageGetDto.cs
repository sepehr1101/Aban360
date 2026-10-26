namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries
{
    public record TicketMessageGetDto
    {
        public long Id { get; set; }
        public long TicketId { get; set; }
        public Guid SenderUserId { get; set; }
        public string? Body { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
