using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts
{
    public interface IZoneCreateHandler
    {
        Task Handle(ZoneCreateDto createDto, CancellationToken cancellationToken);
    }
}
