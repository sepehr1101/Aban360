namespace Aban360.BlobPool.Domain.Features.DmsServices.Dto.Queries
{
    public record SetOnMetadataInputDto
    {
        public string Body { get; set; }
        public string NodeId { get; set; }
        public string GroupName { get; set; }
    }
}
