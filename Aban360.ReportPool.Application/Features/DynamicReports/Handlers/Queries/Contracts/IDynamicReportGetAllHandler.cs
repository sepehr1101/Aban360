using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Queries;

namespace Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Queries.Contracts
{
    public interface IDynamicReportGetAllHandler
    {
        Task<ICollection<DynamicReportGetDto>> Handle(CancellationToken cancellationToken);
    }
}
