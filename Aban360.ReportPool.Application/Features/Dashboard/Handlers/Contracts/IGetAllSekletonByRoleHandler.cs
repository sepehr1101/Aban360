using Aban360.ReportPool.Domain.Features.Dashboard.Entities;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    public interface IGetAllSekletonByRoleHandler
    {
        Task<IEnumerable<Skeleton>?> Handle(string role, CancellationToken cancellationToken);
    }
}