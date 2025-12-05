using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.QueryServices;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Implementations
{
    internal sealed class ZoneAllHandler : IZoneAllHandler
    {
        private readonly IZoneQueryService _zoneQueryService;
        private readonly ICommonZoneService _commonZoneService;
        public ZoneAllHandler(
            IZoneQueryService zoneQueryService,
            ICommonZoneService commonZoneService)
        {
            _zoneQueryService = zoneQueryService;
            _zoneQueryService.NotNull(nameof(zoneQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ICollection<UserZoneIdsOutputDto>> Handle(IAppUser currentUser, CancellationToken cancellationToken)
        {
            IEnumerable<int> userZoneIds = await _commonZoneService.GetMyZoneIds(currentUser);
            IEnumerable<UserZoneIdsOutputDto> zones = await _zoneQueryService.Get();
            ICollection<UserZoneIdsOutputDto> finalZones = zones.Where(zone => userZoneIds.Contains(zone.Id)).ToList();
            return finalZones;
        }
    }
}
