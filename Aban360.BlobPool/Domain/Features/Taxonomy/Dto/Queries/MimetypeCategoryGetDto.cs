namespace Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries
{
    public record MimetypeCategoryGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
