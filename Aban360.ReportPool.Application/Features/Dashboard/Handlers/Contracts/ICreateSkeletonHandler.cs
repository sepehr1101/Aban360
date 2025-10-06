using Aban360.Common.ApplicationUser;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts
{
    public interface ICreateSkeletonHandler
    {
        Task<int> Handle(SkeletonDto skeletonDto, IAppUser currentUser, CancellationToken cancellationToken);
    }
}