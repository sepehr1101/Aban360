namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries
{
    public record TicketAttachmentContentGetDto
    {
        public long Id { get; set; }
        public string OriginalFileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] FileContent { get; set; } = null!;
    }
}
