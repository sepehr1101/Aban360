using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.WaterReturn.Handlers.Queries.Implementations
{
    internal sealed class ReturedBillAfterDateLockGetHandler : IReturedBillAfterDateLockGetHandler
    {
        private readonly IBedBesQueryService _bedBedQueryService;
        private readonly ICommonZoneService _zoneService;
        public ReturedBillAfterDateLockGetHandler(IBedBesQueryService bedBedQueryService, ICommonZoneService zoneService)
        {
            _bedBedQueryService = bedBedQueryService;
            _bedBedQueryService.NotNull(nameof(bedBedQueryService));

            _zoneService = zoneService;
            _zoneService.NotNull(nameof(zoneService));
        }

        public async Task<IEnumerable<ReturnedBillsOutputDto>> Handle(int zoneId, IAppUser appUser, CancellationToken cancellationToken)
        {
            await _zoneService.IsUserInZone(appUser, zoneId);
            IEnumerable<ReturnedBillsOutputDto> result = await _bedBedQueryService.Returned(zoneId);
            return result;
        }
    }
}
