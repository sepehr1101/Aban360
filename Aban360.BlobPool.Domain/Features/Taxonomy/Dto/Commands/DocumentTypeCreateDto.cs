namespace Aban360.BlobPool.Domain.Features.Taxonomy.Dto.Commands
{
    public record DocumentTypeCreateDto
    {
        public short Id { get; set; }
        public short DocumentCategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string Css { get; set; } = null!;
    }
}
