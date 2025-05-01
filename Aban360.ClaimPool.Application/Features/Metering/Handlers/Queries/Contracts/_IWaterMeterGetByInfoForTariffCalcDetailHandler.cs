using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts
{
    public interface _IWaterMeterGetByInfoForTariffCalcDetailHandler
    {
        Task<ICollection<WaterMeterGetDto>> Handle(string fromDate, string toDate, short usageId
            , short individualTypeId, string fromBillId, string toBillId, int zoneId, CancellationToken cancellationToken);

    }
}
