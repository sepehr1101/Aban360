using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Commands;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Delete.Contracts
{
    public interface IDynamicReportDeleteHandler
    {
        Task Handle(DynamicReportDeleteDto deleteDto, CancellationToken cancellationToken);
    }
}
