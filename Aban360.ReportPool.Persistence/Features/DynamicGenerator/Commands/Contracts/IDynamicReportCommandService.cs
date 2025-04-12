using Aban360.ReportPool.Domain.Features.DynamicGenerator.Entities;

namespace Aban360.ReportPool.Persistence.Features.DynamicGenerator.Commands.Contracts
{
    public interface IDynamicReportCommandService
    {
        Task Add(DynamicReport dynamicReport);
        Task Remove(DynamicReport dynamicReport);
    }
}
