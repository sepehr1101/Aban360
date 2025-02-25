namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record UserCreateDto
    {
        public string FullName { get; init; } = null!;
        public string DisplayName { get; init; } = null!;
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string Mobile { get; init; } = null!;
        public int[]? SelectedRoleIds { get; init; }
        public int[]? SelectedZoneIds { get; set; }
        public int[]? SelectedEndpointIds { get; set; }
    }
}