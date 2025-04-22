using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Queries;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Queries.Contracts
{
    public interface IDynamicReportGetMastersHandler
    {
        Task<ICollection<DynamicReportMasterDto>> Handle(CancellationToken cancellationToken);
    }
}
