using Aban360.Common.ApplicationUser;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts
{
    public interface IDeleteSkeletonHandler
    {
        Task<bool> Handle(int id, IAppUser currentUser, CancellationToken cancellationToken);
    }
}