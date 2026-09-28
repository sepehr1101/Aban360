using Aban360.Common.ApplicationUser;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts
{
    public interface IBillInstallmentRemoveHandle
    {
        Task Handle(BillInstallmentRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
