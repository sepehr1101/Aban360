using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Handlers.Queries.Contracts
{
    public interface IReadingBoundGetSingleHandler
    {
        Task<ReadingBoundGetDto> Handle(short id, CancellationToken cancellationToken);
    }
}
