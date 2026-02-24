using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class CartableHandler : ICartableHandler
    {
        private readonly IMeterFlowQueryService _meterFlowService;
        private const int _expirePercent = 50;
        public CartableHandler(IMeterFlowQueryService meterFlowService)
        {
            _meterFlowService = meterFlowService;
            _meterFlowService.NotNull(nameof(meterFlowService));
        }

        public async Task<IEnumerable<MeterFlowCartableGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<MeterFlowCartableGetDto> cartable = await _meterFlowService.GetCartable();
            return cartable;
        }
    }
}
