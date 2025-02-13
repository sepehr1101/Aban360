using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Commands.Create.Contracts
{
    public interface IRegionCreateHandler
    {
        Task Handle(RegionCreateDto createDto, CancellationToken cancellationToken);
    }
}
