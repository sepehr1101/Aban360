namespace Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Queries
{
    public record DynamicReportGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public long Version { get; set; }

        public string? Description { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; } = null!;

        public Guid DocumentId { get; set; }

    }
}
