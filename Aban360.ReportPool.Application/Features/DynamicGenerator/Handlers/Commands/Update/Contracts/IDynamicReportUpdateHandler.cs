using Aban360.Common.ApplicationUser;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Commands;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Update.Contracts
{
    public interface IDynamicReportUpdateHandler
    {
        Task Handle(IAppUser currentUser, DynamicReportUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
