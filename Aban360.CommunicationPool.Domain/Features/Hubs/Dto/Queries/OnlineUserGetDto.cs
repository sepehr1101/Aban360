namespace Aban360.CommunicationPool.Domain.Features.Hubs.Dto.Queries
{
    public record OnlineUserGetDto
    {
        public long Id { get; set; }
        public string ConnectionId { get; set; }
        public Guid UserId { get; set; }
        public string FullName{ get; set; }
        public DateTime ConnectDateTime { get; set; }

    }
}
