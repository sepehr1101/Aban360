namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries
{
    public record AppWithModuleGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Style { get; set; } = null!;
        public bool InMenu { get; set; }
        public int LogicalOrder { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<ModuleGetDto> Modules { get; set; }
    }
}
