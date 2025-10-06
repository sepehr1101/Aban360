namespace Aban360.ReportPool.Domain.Features.Dashboard.Entities
{
    public class Skeleton
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int RoleId { get; set; }
        public string RoleTitle { get; set; } = default!;
        public string? Content { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string CreatedBy { get; set; }=default!;
        public DateTime? DeleteDateTime { get; set; }
        public string? DeletedBy { get; set; }
    }
}
