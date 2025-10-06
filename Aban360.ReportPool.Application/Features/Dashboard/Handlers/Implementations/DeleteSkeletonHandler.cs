using Aban360.Common.ApplicationUser;
using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    internal sealed class DeleteSkeletonHandler : IDeleteSkeletonHandler
    {
        private readonly ISkeletonService _skeletonService;

        public DeleteSkeletonHandler(ISkeletonService skeletonService)
        {
            _skeletonService = skeletonService;
        }

        public async Task<bool> Handle(int id, IAppUser currentUser, CancellationToken cancellationToken)
        {
            return await _skeletonService.Delete(id, currentUser.FullName);
        }
    }
}
