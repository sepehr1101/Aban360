namespace Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Queries
{
    public record DynamicReportMasterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public long Version { get; set; }
        public string? Description { get; set; }
        public string UserDisplayName { get; set; } = null!;
    }
}
