using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    internal sealed class GetSkeletonDefinitionHandler : IGetSkeletonDefinitionHandler
    {
        private readonly ISkeletonService _skeletonService;
        public GetSkeletonDefinitionHandler(ISkeletonService skeletonService)
        {
            _skeletonService = skeletonService;
            _skeletonService.NotNull(nameof(skeletonService));
        }
        public async Task<IEnumerable<SkeletonDefinitionDto>> Handle(CancellationToken cancellationToken)
        {
            return await _skeletonService.GetDefinitions();
        }
    }
}
