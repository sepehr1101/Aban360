namespace Aban360.ReportPool.Domain.Features.Dashboard.Dtos
{
    public class SkeletonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int RoleId { get; set; }
        public string RoleTitle { get; set; } = default!;
        public string? Content { get; set; }
    }
}
