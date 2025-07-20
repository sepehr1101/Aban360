using System.ComponentModel.DataAnnotations.Schema;

namespace Aban360.CommunicationPool.Domain.Features.Hubs.Entities
{
    [Table(nameof(HubEvent))]
    public class HubEvent
    {
        public long Id { get; set; }
        public string ConnectionId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ConnectDateTime { get; set; }
        public DateTime? DisconnectDateTime { get; set; }
    }
}
