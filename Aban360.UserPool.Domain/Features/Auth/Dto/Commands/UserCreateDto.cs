using Aban360.UserPool.Domain.Features.Auth.Dto.Base;

namespace Aban360.UserPool.Domain.Features.Auth.Dto.Commands
{
    public record UserCreateDto
    {
        public string FullName { get; init; } = null!;
        public string DisplayName { get; init; } = null!;
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string Mobile { get; init; } = null!;
        public ICollection<int>? RoleIds { get; init; }
        public ICollection<ClaimDto>? ClaimItems { get; init; }
    }
}