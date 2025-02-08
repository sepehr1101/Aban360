namespace Aban360.UserPool.Domain.Features.Auth.Dto.Queries
{
    public record RoleGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? DefaultClaims { get; set; }
        public bool SensitiveInfo { get; set; }
        public bool IsRemovable { get; set; }
        public int? PreviousId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string InsertLogInfo { get; set; } = null!;
        public string? RemoveLogInfo { get; set; }
    }
}
