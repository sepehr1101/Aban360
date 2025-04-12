using Aban360.ReportPool.Domain.Features.DynamicReports.Entities;

namespace Aban360.ReportPool.Persistence.Features.DynamicReports.Commands.Contracts
{
    public interface IDynamicReportCommandService
    {
        Task Add(DynamicReport dynamicReport);
        Task Remove(DynamicReport dynamicReport);
    }
}
