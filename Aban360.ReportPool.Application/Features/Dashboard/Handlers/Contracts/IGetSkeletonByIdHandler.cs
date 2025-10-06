using Aban360.ReportPool.Domain.Features.Dashboard.Entities;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts
{
    public interface IGetSkeletonByIdHandler
    {
        Task<Skeleton?> Handle(int id, CancellationToken cancellationToken);
    }
}