namespace Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Commands
{
    public record HubCloseConnectionsDto
    {
        public Guid UserId { get; }
        public HubCloseConnectionsDto(Guid userId)
        {
            UserId = userId;
        }
        public static implicit operator HubCloseConnectionsDto(Guid userId) => new(userId);
        public static implicit operator Guid(HubCloseConnectionsDto hubCloseConnectionsDto) => hubCloseConnectionsDto.UserId;
    }
}
