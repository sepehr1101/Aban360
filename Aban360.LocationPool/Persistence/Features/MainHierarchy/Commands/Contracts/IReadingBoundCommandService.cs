using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts
{
    public interface IReadingBoundCommandService
    {
        Task Add(ReadingBound readingBound);
        Task Remove(ReadingBound readingBound);
    }
}
