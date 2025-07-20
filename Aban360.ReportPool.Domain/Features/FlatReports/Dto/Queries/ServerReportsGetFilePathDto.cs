namespace Aban360.ReportPool.Domain.Features.FlatReports.Dto.Queries
{
    public record ServerReportsGetFilePathDto
    {
        public Guid Id { get; set; }
        public string ReportPath { get; set; }
    }
}
