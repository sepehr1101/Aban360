using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts
{
    public interface IWaterMeterCreateHandler
    {
        Task Handle(WaterMeterCreateDto createDto, CancellationToken cancellationToken);
    }
}
