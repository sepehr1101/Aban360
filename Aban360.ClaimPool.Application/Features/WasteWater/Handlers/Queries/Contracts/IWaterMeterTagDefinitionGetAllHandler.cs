using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts
{
    public interface IWaterMeterTagDefinitionGetAllHandler
    {
        Task<ICollection<WaterMeterTagDefinitionGetDto>> Handle(CancellationToken cancellationToken);
    }
}
