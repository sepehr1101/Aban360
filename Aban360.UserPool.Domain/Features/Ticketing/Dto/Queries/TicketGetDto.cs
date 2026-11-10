namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries
{
    public record TicketGetDto
    {
        public long Id { get; set; }
        // Presentation only; all commands and authorization continue to use Id.
        public string RequestNumber => Id.ToString("D8", System.Globalization.CultureInfo.InvariantCulture);
        public string TicketNumber { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public Guid CreatedByUserId { get; set; }
        public string? CreatedByUserFullName { get; set; }
        public string CreatedByUserRoles { get; set; } = string.Empty;
        public Guid? AssignedToUserId { get; set; }
        public short StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
    }
}
