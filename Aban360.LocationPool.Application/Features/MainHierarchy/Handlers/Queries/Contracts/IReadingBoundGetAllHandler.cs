using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts
{
    public interface IReadingBoundGetAllHandler
    {
        Task<ICollection<ReadingBoundGetDto>> Handle( CancellationToken cancellationToken);
    }
}
