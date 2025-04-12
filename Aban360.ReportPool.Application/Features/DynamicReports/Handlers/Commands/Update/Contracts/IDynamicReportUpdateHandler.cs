using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Commands;

namespace Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Commands.Update.Contracts
{
    public interface IDynamicReportUpdateHandler
    {
        Task Handle(DynamicReportUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
