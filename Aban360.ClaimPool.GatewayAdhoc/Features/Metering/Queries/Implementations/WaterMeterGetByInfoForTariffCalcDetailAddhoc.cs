using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.GatewayAdhoc.Features.Metering.Queries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.ClaimPool.GatewayAdhoc.Features.Metering.Queries.Implementations
{
    internal sealed class WaterMeterGetByInfoForTariffCalcDetailAddhoc : IWaterMeterGetByInfoForTariffCalcDetailAddhoc
    {
        private readonly I_WaterMeterGetByInfoForTariffCalcDetailHandler _handler;
        public WaterMeterGetByInfoForTariffCalcDetailAddhoc(I_WaterMeterGetByInfoForTariffCalcDetailHandler handler)
        {
            _handler = handler;
            _handler.NotNull(nameof(handler));
        }

        public async Task<ICollection<WaterMeterGetDto>> Handle(string fromDate, string toDate, short usageId,
            short individualTypeId, string fromBillId, string toBillId, int zoneId, CancellationToken cancellationToken)
        {
            var waterMeters = await _handler.Handle(fromDate, toDate, usageId
             , individualTypeId, fromBillId, toBillId, zoneId, cancellationToken);

            return waterMeters;
        }
    }
}
