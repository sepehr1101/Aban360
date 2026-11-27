using Aban360.MeterPool.Domain.Features.Management.Entities;

namespace Aban360.MeterPool.Persistence.Features.Manegement.Commands.Contracts
{
    public interface IReadingPeriodTypeCommandService
    {
        Task Add(ReadingPeriodType readingPeriodType);
        void Remove(ReadingPeriodType readingPeriodType);
    }
}
