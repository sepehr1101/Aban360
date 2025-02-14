namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record UserRoleQueryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public bool IsSelected { get; set; }
    }
    public record UserRoleInfo
    {
        public ICollection<UserRoleQueryDto>? RoleInfo { get;}
        public UserRoleInfo(ICollection<UserRoleQueryDto> userRoleQueryDtos)
        {
            RoleInfo = userRoleQueryDtos;
        }
        public UserRoleInfo()
        {
            RoleInfo= new HashSet<UserRoleQueryDto>();
        }
    }
}
