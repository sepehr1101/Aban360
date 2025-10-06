namespace Aban360.ReportPool.Domain.Features.Dashboard.Dtos
{
    public class SkeletonDefinitionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int RoleId { get; set; }
        public string RoleTitle { get; set; } = default!;
    }
}
