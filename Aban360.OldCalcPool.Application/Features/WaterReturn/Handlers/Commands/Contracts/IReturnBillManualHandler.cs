using Aban360.Common.ApplicationUser;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IReturnBillManualHandler
    {
        Task Handle(ReturnBillManualInputDto inputDto, IAppUser appUser, CancellationToken cancellationToken);
    }
}