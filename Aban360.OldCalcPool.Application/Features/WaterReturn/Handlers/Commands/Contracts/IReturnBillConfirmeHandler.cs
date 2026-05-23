using Aban360.Common.ApplicationUser;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPools.Domain.Features.WaterReturn.Dto.Commands;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Commands.Contracts
{
    public interface IReturnBillConfirmeHandler
    {
        Task<ReturnBillDataOutputDto> Handle(ReturnBillConfirmeByBillIdInputDto input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
