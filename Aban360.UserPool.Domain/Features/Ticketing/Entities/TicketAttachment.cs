using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Ticketing.Entities
{
    [Table(nameof(TicketAttachment))]
    public class TicketAttachment
    {
        public long Id { get; set; }
        public long TicketMessageId { get; set; }
        // ROWGUIDCOL and FILESTREAM storage are configured in the persistence layer.
        public Guid FileGuid { get; set; }
        public string OriginalFileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long SizeBytes { get; set; }
        public byte[] FileContent { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public virtual TicketMessage TicketMessage { get; set; } = null!;
    }
}
