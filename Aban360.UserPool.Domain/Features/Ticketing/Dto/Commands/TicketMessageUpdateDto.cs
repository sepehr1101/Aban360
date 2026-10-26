namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands
{
    public record TicketMessageUpdateDto
    {
        public long Id { get; set; }
        public string? Body { get; set; }
    }
}
