using Aban360.LocationPool.Domain.Features.MainHierarchy;

namespace Aban360.LocationPool.Persistence.Features.MainHierarchy.Commands.Contracts
{
    public interface IReadingBlockCommandService
    {
        Task Add(ReadingBlock readingBlock);
        Task Remove(ReadingBlock readingBlock);
    }
}
