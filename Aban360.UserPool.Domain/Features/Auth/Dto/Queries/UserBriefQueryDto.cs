namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record UserBriefQueryDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = null!;
    }
}
