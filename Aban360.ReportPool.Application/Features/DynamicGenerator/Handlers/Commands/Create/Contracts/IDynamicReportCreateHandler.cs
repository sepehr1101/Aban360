using Aban360.Common.ApplicationUser;
using Aban360.ReportPool.Domain.Features.DynamicGenerator.Dto.Commands;

namespace Aban360.ReportPool.Application.Features.DynamicGenerator.Handlers.Commands.Create.Contracts
{
    public interface IDynamicReportCreateHandler
    {
        Task Handle(IAppUser currentUser, DynamicReportCreateDto createDto, CancellationToken cancellationToken);
    }
}
