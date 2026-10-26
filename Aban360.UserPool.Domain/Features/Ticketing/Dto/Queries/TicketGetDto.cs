namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries
{
    public record TicketGetDto
    {
        public long Id { get; set; }
        public string TicketNumber { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public Guid CreatedByUserId { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public short StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
