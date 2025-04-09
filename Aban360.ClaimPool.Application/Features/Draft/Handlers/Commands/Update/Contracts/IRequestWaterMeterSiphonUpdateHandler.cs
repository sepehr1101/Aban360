using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;

namespace Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts
{
    public interface IRequestWaterMeterSiphonUpdateHandler
    {
        Task Handle(WaterMeterSiphonRequestUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
