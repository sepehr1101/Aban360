namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands
{
    public record TicketCreateDto
    {
        public string Subject { get; set; } = null!;
        public string? Body { get; set; }
        public ICollection<TicketFileCreateDto> Attachments { get; set; } = new List<TicketFileCreateDto>();
    }
}
