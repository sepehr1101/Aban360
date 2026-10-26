namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands
{
    public record TicketRatingCreateDto
    {
        public long TicketMessageId { get; set; }
        public byte Score { get; set; }
        public string? Comment { get; set; }
    }
}
