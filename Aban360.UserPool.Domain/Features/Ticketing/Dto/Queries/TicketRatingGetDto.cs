namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries
{
    public record TicketRatingGetDto
    {
        public long Id { get; set; }
        public long TicketMessageId { get; set; }
        public Guid RatedByUserId { get; set; }
        public byte Score { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
