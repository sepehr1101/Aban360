namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands
{
    public record TicketMessageCreateDto
    {
        public long TicketId { get; set; }
        public string? Body { get; set; }
        public ICollection<TicketFileCreateDto> Attachments { get; set; } = new List<TicketFileCreateDto>();
    }
}
