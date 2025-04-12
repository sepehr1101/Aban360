using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Queries;

namespace Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Queries.Contracts
{
    public interface IDynamicReportGetSingleHandler
    {
        Task<DynamicReportGetDto> Handle(int id, CancellationToken cancellationToken);
    }
}
