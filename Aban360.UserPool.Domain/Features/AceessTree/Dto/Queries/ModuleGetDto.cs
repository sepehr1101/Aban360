namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries
{
    public record ModuleGetDto
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public string AppTitle { get; set; } = default!;
        public string Title { get; set; } = null!;
        public string? Style { get; set; }
        public bool InMenu { get; set; }
        public int LogicalOrder { get; set; }
        public string? ClientRoute { get; set; }
        public string? Description { get; set; }
    }
}
