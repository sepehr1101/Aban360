using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts
{
    public interface IWaterMeterTagGetSingleHandler
    {
        Task<ICollection<WaterMeterTagGetDto>> Handle(string billId, CancellationToken cancellationToken);

    }
}
