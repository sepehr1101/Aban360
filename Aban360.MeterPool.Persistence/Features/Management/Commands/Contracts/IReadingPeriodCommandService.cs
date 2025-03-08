using Aban360.MeterPool.Domain.Features.Management.Entities;

namespace Aban360.MeterPool.Persistence.Features.Manegement.Commands.Contracts
{
    public interface IReadingPeriodCommandService
    {
        Task Add(ReadingPeriod readingPeriod);
        Task Remove(ReadingPeriod readingPeriod);
    }
}
