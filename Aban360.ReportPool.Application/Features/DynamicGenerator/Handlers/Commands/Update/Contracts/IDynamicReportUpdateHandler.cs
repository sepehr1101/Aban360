using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Commands;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Update.Contracts
{
    public interface IDynamicReportUpdateHandler
    {
        Task Handle(DynamicReportUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
