using Aban360.UserPool.Domain.Features.Auth.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Ticketing.Entities
{
    [Table(nameof(TicketRating))]
    public class TicketRating
    {
        public long Id { get; set; }
        public long TicketMessageId { get; set; }
        public Guid RatedByUserId { get; set; }
        public byte Score { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual TicketMessage TicketMessage { get; set; } = null!;
        public virtual User RatedByUser { get; set; } = null!;
    }
}
