namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ConnectDisconnectRemoveDto
    {
        public long Id { get; set; }
        public DateTime RemovedDateTime { get; set; }
        public Guid RemovedBy { get; set; }
        public string? Description { get; set; }
        public ConnectDisconnectRemoveDto(long id,DateTime removedDateTime,Guid removedBy,string? description)
        {
            Id= id;
            RemovedDateTime = removedDateTime;
            RemovedBy = removedBy;
            Description = description;
        }
        public ConnectDisconnectRemoveDto()
        {
        }
    }
}
