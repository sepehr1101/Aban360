using Aban360.MeterPool.Domain.Features.Management.Entities;

namespace Aban360.MeterPool.Persistence.Features.Management.Commands.Contracts
{
    public interface IReadingConfigDefaultCommandService
    {
        Task Add(ReadingConfigDefault readingConfigDefault);
        Task Remove(ReadingConfigDefault readingConfigDefault);
    }
}
