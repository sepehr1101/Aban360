namespace Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands
{
    public record ExecutableMimetypeCreateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public bool StreamingOption { get; set; }
        public bool FrontendExecutor { get; set; }
    }
}
