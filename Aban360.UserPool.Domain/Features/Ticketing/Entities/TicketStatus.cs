using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.UserPool.Domain.Features.Ticketing.Entities
{
    [Table(nameof(TicketStatus))]
    public class TicketStatus
    {
        public short Id { get; set; }
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
