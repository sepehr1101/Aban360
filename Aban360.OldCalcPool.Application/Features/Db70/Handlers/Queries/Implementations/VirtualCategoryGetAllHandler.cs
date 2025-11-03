using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;
using FluentValidation;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations
{
    internal sealed class VirtualCategoryGetAllHandler : IVirtualCategoryGetAllHandler
    {
        private readonly IVirtualCategoryQueryService _virtualCategoryQueryService;

        public VirtualCategoryGetAllHandler(IVirtualCategoryQueryService virtualCategoryQueryService)
        {
            _virtualCategoryQueryService = virtualCategoryQueryService;
            _virtualCategoryQueryService.NotNull(nameof(virtualCategoryQueryService));
        }
        public async Task<IEnumerable<VirtualCategoryGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<VirtualCategoryGetDto> result = await _virtualCategoryQueryService.Get();
            return result;
        }
    }
}
