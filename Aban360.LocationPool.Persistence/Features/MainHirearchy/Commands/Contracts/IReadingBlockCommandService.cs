using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts
{
    public interface IReadingBlockCommandService
    {
        Task Add(ReadingBlock readingBlock);
        Task Remove(ReadingBlock readingBlock);
    }
}
