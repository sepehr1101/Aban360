using Aban360.MeterPool.Domain.Features.Management.Dtos.Queries;

namespace Aban360.MeterPool.Application.Features.Management.Handlers.Queries.Contracts
{
    public interface IReadingPeriodGetAllHandler
    {
        Task<ICollection<ReadingPeriodGetDto>> Handle(CancellationToken cancellationToken);
    }
}
