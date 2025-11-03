namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record SearchUserDto
    {
        public ICollection<int> ZoneIds { get; set; }
        public ICollection<int> EndpointIds { get; set; }
        public ICollection<int> RoleIds { get; set; }
    }
}
