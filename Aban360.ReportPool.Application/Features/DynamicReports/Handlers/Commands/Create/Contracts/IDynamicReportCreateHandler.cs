using Aban360.ReportPool.Domain.Features.DynamicReports.Dto.Commands;

namespace Aban360.ReportPool.Application.Features.DynamicReports.Handlers.Commands.Create.Contracts
{
    public interface IDynamicReportCreateHandler
    {
        Task Handle(DynamicReportCreateDto createDto, CancellationToken cancellationToken);
    }
}
