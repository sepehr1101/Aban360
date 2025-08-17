namespace Aban360.BlobPool.Domain.Features.DmsServices.Dto.Queries
{
    public record SearchUserInputDto
    {
        public string FolderPath { get; set; }
        public string Property { get; set; }
        public string Path { get; set; }
    }
}
