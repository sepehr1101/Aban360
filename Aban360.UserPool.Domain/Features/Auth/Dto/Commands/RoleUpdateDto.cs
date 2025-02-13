namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record RoleUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? DefaultClaims { get; set; }
        public bool SensitiveInfo { get; set; }
        public int[]? SelectedEndpointIds { get; set; }
    }
}
