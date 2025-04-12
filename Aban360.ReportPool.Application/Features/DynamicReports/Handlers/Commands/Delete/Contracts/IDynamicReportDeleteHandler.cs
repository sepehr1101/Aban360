using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Commands;

namespace Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Commands.Delete.Contracts
{
    public interface IDynamicReportDeleteHandler
    {
        Task Handle(DynamicReportDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
