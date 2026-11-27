using Aban360.MeterPool.Domain.Features.Reading;

namespace Aban360.MeterPool.Application.Features.Reading.Handlers.Queries.Contracts
{
    public interface ILoad
    {
        Task<IReadOnlyCollection<ReadingLoadDto>> Handle(CancellationToken cancellationToken);
    }
}
