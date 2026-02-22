namespace Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands
{
    public record AddOrUpdateMetaDataDto
    {
        public int? section { get; set; }
        public int? city { get; set; }
        public int? village { get; set; }
        public int? title { get; set; }
    }
}
