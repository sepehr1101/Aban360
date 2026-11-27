namespace Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands
{
    public record HubEventUpdateDto
    {
        public string ConnectionId { get; set; }
        public HubEventUpdateDto(string connectionId)
        {
            ConnectionId = connectionId;
        }
    }
}
