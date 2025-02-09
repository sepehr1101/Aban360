namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries
{
    public record EndpointGetDto
    {
        public int Id { get; set; }
        public int SubModuleId { get; set; }
        public string SubModuleTitle { get; set; }
        public string Title { get; set; } = null!;
        public string? Style { get; set; }
        public bool InMenu { get; set; }
        public int LogicalOrder { get; set; }
        public string? AuthValue { get; set; }
        public bool IsActive { get; set; }
    }
}
