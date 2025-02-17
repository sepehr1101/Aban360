using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts
{
    public interface IWaterMeterTagGetSinglBySearchInputeHandler
    {
        Task<WaterMeterTagGetDto> Handle(string input, CancellationToken cancellationToken);

    }
}
