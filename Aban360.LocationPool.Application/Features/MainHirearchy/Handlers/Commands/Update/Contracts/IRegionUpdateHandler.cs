using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Commands.Update.Contracts
{
    public interface IRegionUpdateHandler
    {
        Task Handle(RegionUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
