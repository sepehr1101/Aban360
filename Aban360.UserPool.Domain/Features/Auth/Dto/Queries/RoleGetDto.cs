using Aban360.UserPool.Domain.Features.AceessTree.Dto.Queries.ValueKeyItems;

namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record RoleGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? DefaultClaims { get; set; }
        public bool SensitiveInfo { get; set; }
        public int[]? SelectedEndpointIds { get; set; }
        public AccessTreeValueKeyDto? AccessTreeValueKeyDto { get; set; }
    }
}
