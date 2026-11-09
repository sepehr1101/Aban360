namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands
{
    public record TicketCreateResultDto
    {
        public long Id { get; init; }
        public string TicketNumber { get; init; } = null!;
        public string RequestNumber => Id.ToString("D8", System.Globalization.CultureInfo.InvariantCulture);
    }
}
