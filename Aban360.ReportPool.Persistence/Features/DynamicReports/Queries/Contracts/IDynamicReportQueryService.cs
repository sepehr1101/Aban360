using Aban360.ReportPool.Domain.Features.DynamicReports.Entities;

namespace Aban360.ReportPool.Persistence.Features.DynamicReports.Queries.Contracts
{
    public interface IDynamicReportQueryService
    {
        Task<DynamicReport> Get(int id);
        Task<ICollection<DynamicReport>> Get();
    }
}
