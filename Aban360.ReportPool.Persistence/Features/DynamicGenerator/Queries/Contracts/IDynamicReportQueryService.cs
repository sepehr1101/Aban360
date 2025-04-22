using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Queries;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Entities;

namespace Aban360.ReportPool.Persistence.Features.DynamicGenerator.Queries.Contracts
{
    public interface IDynamicReportQueryService
    {
        Task<DynamicReport> Get(int id);
        Task<string> GetTemplateJson(int id);
        Task<ICollection<DynamicReportMasterDto>> GetMasters();
    }
}
