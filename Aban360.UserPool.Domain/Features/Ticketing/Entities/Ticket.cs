using Aban360.UserPool.Domain.Features.Auth.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Ticketing.Entities
{
    [Table(nameof(Ticket))]
    public class Ticket
    {
        public long Id { get; set; }
        public string TicketNumber { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public Guid CreatedByUserId { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public short StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public virtual User CreatedByUser { get; set; } = null!;

        [ForeignKey(nameof(AssignedToUserId))]
        public virtual User? AssignedToUser { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual TicketStatus Status { get; set; } = null!;

        public virtual ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
    }
}
