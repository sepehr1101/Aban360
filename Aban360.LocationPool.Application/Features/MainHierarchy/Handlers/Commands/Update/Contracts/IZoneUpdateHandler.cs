using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Update.Contracts
{
    public interface IZoneUpdateHandler
    {
        Task Handle(ZoneUpdateDto updateDto, CancellationToken cancellationToken);
    }
}
