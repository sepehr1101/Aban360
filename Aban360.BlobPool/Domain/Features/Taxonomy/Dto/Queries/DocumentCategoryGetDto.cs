namespace Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Queries
{
    public record DocumentCategoryGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Icon { get; set; }
        public string? Css { get; set; }
    }
}
