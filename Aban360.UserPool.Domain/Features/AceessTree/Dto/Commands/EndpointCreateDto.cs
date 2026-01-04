namespace Aban360.UserPool.Domain.Features.AceessTree.Dto.Commands
{
    public record EndpointCreateDto
    {
        public int SubModuleId { get; set; }
        public string Title { get; set; } = null!;
        public string? Style { get; set; }
        public bool InMenu { get; set; }
        public int LogicalOrder { get; set; }
        public string? AuthValue { get; set; }
    }
}
