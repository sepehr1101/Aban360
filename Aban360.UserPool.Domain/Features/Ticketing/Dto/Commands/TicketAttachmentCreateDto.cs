namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands
{
    public record TicketAttachmentCreateDto : TicketFileCreateDto
    {
        public long TicketMessageId { get; set; }
    }
}
