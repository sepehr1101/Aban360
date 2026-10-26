namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries
{
    public record TicketStatusGetDto
    {
        public short Id { get; set; }
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;
    }
}
