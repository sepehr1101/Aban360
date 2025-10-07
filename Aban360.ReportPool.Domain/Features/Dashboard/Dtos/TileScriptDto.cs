namespace Aban360.ReportPool.Domain.Features.Dashboard.Dtos
{
    public class TileScriptDto
    {
        public int Id { get; set; }
        public int ReportCode { get; set; }
        public string? Description { get; set; }
        public string Content { get; set; } = default!;
    }
}
