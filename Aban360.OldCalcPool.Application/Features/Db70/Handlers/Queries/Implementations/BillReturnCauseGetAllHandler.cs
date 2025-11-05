using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Db70.Dto.Queries;
using Aban360.OldCalcPool.Persistence.Features.Db70.Queries.Contracts;

namespace Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations
{
    internal sealed class BillReturnCauseGetAllHandler : IBillReturnCauseGetAllHandler
    {
        private readonly IBillReturnCauseQueryService _virtualCategoryQueryService;

        public BillReturnCauseGetAllHandler(IBillReturnCauseQueryService virtualCategoryQueryService)
        {
            _virtualCategoryQueryService = virtualCategoryQueryService;
            _virtualCategoryQueryService.NotNull(nameof(virtualCategoryQueryService));
        }
        public async Task<IEnumerable<BillReturnCauseGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<BillReturnCauseGetDto> result = await _virtualCategoryQueryService.Get();
            return result;
        }
    }
}
