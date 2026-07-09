using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class CartableHandler : ICartableHandler
    {
        private readonly IMeterFlowQueryService _meterFlowService;
        private readonly ICommonZoneService _commonZoneService;
        public CartableHandler(
            IMeterFlowQueryService meterFlowService,
            ICommonZoneService commonZoneService)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<IEnumerable<MeterFlowCartableGetDto>> Handle(IAppUser appUser, CancellationToken cancellationToken)
        {
            IEnumerable<int> zoneIds = await _commonZoneService.GetMyZoneIds(appUser);
            IEnumerable<MeterFlowCartableGetDto> cartable = await _meterFlowService.GetCartable(zoneIds);
            return cartable;
        }
    }
}
