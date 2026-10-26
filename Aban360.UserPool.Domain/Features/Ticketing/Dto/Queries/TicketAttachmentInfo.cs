namespace Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries
{
    public class TicketAttachmentInfo
    {
        public long Id { get; set; }
        public long TicketMessageId { get; set; }
        public string OriginalFileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long SizeBytes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
