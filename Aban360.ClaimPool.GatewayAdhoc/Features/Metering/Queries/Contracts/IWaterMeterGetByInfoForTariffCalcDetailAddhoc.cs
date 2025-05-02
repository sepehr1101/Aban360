using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;

namespace Aban360.ClaimPool.GatewayAdhoc.Features.Metering.Queries.Contracts
{
    public interface IWaterMeterGetByInfoForTariffCalcDetailAddhoc
    {
        Task<ICollection<WaterMeterGetDto>> Handle(string fromDate, string toDate, short usageId
                    , short individualTypeId, string fromBillId, string toBillId, int zoneId, CancellationToken cancellationToken);
    }
}
