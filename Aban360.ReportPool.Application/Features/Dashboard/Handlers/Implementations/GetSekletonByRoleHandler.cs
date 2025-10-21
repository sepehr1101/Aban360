using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    internal sealed class GetSekletonByRoleHandler : IGetSekletonByRoleHandler
    {
        private readonly ISkeletonService _skeletonService;
        public GetSekletonByRoleHandler(ISkeletonService skeletonService)
        {
            _skeletonService = skeletonService;
            _skeletonService.NotNull();
        }
        public async Task<Skeleton?> Handle(string role, CancellationToken cancellationToken)
        {
            return await _skeletonService.GetByRole(role);
        }
    }
}
