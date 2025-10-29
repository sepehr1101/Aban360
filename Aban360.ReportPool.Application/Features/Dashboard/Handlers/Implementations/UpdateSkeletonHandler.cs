using Aban360.Common.ApplicationUser;
using Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;
using Aban360.ReportPool.Domain.Features.Dashboard.Entities;
using Aban360.ReportPool.Persistence.Features.Dashboard.Contracts;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Implementations
{
    internal sealed class UpdateSkeletonHandler : IUpdateSkeletonHandler
    {
        private readonly ISkeletonService _skeletonService;

        public UpdateSkeletonHandler(ISkeletonService skeletonService)
        {
            _skeletonService = skeletonService;
        }

        public async Task<bool> Handle(SkeletonDto skeletonDto, IAppUser currentUser, CancellationToken cancellationToken)
        {
            Skeleton skeleton = Map(skeletonDto, currentUser);
            return await _skeletonService.Update(skeleton);
        }
        private Skeleton Map(SkeletonDto skeletonDto, IAppUser currentUser)
        {
            return new Skeleton()
            {
                Id=skeletonDto.Id,
                Content = skeletonDto.Content,
                CreateDateTime = DateTime.Now,
                CreatedBy = currentUser.FullName,
                Name = skeletonDto.Name,
                RoleId = skeletonDto.RoleId,
                RoleTitle = skeletonDto.RoleTitle
            };
        }
    }
}
