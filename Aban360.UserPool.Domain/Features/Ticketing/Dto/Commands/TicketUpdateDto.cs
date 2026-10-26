namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands
{
    public record TicketUpdateDto
    {
        public long Id { get; set; }
        public string Subject { get; set; } = null!;
        public Guid? AssignedToUserId { get; set; }
        public short StatusId { get; set; }
    }
}
