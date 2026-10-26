namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands
{
    public record TicketAttachmentUpdateDto : TicketFileCreateDto
    {
        public long Id { get; set; }
    }
}
