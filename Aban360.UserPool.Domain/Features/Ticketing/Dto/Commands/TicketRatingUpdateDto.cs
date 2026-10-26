namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands
{
    public record TicketRatingUpdateDto
    {
        public long Id { get; set; }
        public byte Score { get; set; }
        public string? Comment { get; set; }
    }
}
