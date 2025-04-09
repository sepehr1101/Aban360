namespace Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands
{
    public record MimetypeCategoryUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
