namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries
{
    public record SubModuleGetDto
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string ModuleTitle { get; set; }
        public string Title { get; set; } = null!;
        public string? Style { get; set; }
        public bool InMenu { get; set; }
        public int LogicalOrder { get; set; }
        public string? ClientRoute { get; set; }
        public bool IsActive { get; set; }
    }
}
