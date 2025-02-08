namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record UserQueryDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public bool MobileConfirmed { get; set; }
        public bool HasTwoStepVerification { get; set; }
        public bool IsLocked { get; set; }
    }
}
