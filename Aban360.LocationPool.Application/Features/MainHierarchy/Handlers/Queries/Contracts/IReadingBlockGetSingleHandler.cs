using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Handlers.Queries.Contracts
{
    public interface IReadingBlockGetSingleHandler
    {
        Task<ReadingBlockGetDto> Handle(short id,CancellationToken cancellationToken);
    }
}
