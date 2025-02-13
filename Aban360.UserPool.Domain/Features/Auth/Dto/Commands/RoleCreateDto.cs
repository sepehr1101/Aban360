namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record RoleCreateDto
    {
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int[]? SelectedEndpointIds { get; set; }
        public bool SensitiveInfo { get; set; }
    }
}
