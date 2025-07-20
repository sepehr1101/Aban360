namespace Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands
{
    public record HubEventCreateDto
    {
        public string ConnectionId { get; set; }
        public Guid UserId { get; set; }
        public DateTime ConnectDateTime { get; set; }
    }
}
