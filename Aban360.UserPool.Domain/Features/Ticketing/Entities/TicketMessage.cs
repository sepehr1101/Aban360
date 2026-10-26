using Aban360.UserPool.Domain.Features.Auth.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Ticketing.Entities
{
    [Table(nameof(TicketMessage))]
    public class TicketMessage
    {
        public long Id { get; set; }
        public long TicketId { get; set; }
        public Guid SenderUserId { get; set; }
        public string? Body { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Ticket Ticket { get; set; } = null!;
        public virtual User SenderUser { get; set; } = null!;
        public virtual ICollection<TicketAttachment> Attachments { get; set; } = new List<TicketAttachment>();
        public virtual TicketRating? Rating { get; set; }
    }
}
