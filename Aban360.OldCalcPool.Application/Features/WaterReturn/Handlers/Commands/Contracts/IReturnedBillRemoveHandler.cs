using Aban360.Common.ApplicationUser;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IReturnedBillRemoveHandler
    {
        Task Handle(WaterReturedRemoveInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}
