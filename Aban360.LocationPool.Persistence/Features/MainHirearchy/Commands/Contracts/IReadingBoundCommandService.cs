using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Persistence.Features.MainHirearchy.Commands.Contracts
{
    public interface IReadingBoundCommandService
    {
        Task Add(ReadingBound readingBound);
        Task Remove(ReadingBound readingBound);
    }
}
