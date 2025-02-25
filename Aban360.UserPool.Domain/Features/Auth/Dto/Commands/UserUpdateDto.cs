namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record UserUpdateDto
    {
        public Guid Id { get; init; }
        public string FullName { get; init; } = null!;
        public string DisplayName { get; init; } = null!;
        public string Mobile { get; init; } = null!;
        public int[]? SelectedRoleIds { get; set; }
        public int[]? SelectedEndpointIds { get; set; }
        public int[]? SelectedZoneIds { get; set; }
    }
}