using Aban360.ReportPool.Domain.Features.Dashboard.Dtos;

namespace Aban360.ReportPool.Application.Features.Dashboard.Handlers.Contracts
{
    public interface IGetSkeletonDefinitionHandler
    {
        Task<IEnumerable<SkeletonDefinitionDto>> Handle(CancellationToken cancellationToken);
    }
}