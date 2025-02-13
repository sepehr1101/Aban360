using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries.ValueKeys;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Implementations
{
    public interface ILocationValueKeyQueryHandler
    {
        Task<LocationValueKeyDto> Handle(CancellationToken cancellationToken);
    }
}