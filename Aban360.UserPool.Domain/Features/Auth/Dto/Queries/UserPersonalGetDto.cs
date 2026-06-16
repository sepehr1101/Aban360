namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record UserPersonalGetDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public UserPersonalGetDto(Guid id, string fullName, string displayName, string userName, string mobile)
        {
            Id = id;
            FullName = fullName;
            DisplayName = displayName;
            Username = userName;
            Mobile = mobile;
        }
        public UserPersonalGetDto()
        {
        }
    }
}
