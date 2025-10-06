namespace Aban360.ReportPool.Domain.Features.Dashboard.Entities
{
    public class TileScript
    {
        public int Id { get; set; }
        public int ReportCode { get; set; }
        public string? Description { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreateDateTime { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime? DeleteDateTime { get; set; }
        public string? DeletedBy { get; set; }
    }
}
