using Aban360.ReportPool.Domain.Features.DynamicGenerator.Entities;

namespace Aban360.ReportPool.Persistence.Features.DynamicGenerator.Queries.Contracts
{
    public interface IDynamicReportQueryService
    {
        Task<DynamicReport> Get(int id);
        Task<ICollection<DynamicReport>> Get();
    }
}
