namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
    using Aban360.ReportPool.Domain.Features.Dashboard.Entities;
    using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;
    using System.Threading.Tasks;

    internal sealed class GetSkeletonByIdHandler : IGetSkeletonByIdHandler
    {
        private readonly ISkeletonService _skeletonService;

        public GetSkeletonByIdHandler(ISkeletonService skeletonService)
        {
            _skeletonService = skeletonService;
        }

        public async Task<Skeleton?> Handle(int id, CancellationToken cancellationToken)
        {
            return await _skeletonService.GetById(id);
        }
    }
}
