using Aban360.Common.ApplicationUser;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts
{
    public interface IReturedBillAfterDateLockGetHandler
    {
        Task<IEnumerable<ReturnedBillsOutputDto>> Handle(int zoneId, IAppUser appUser, CancellationToken cancellationToken);
    }
}
