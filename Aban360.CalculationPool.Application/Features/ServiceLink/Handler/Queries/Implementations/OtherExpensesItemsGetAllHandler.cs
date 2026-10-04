using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Implementations
{
    internal sealed class OtherExpensesItemsGetAllHandler : IOtherExpensesItemsGetAllHandler
    {
        private readonly IOtherExpensesItemsQueryService _otherExpensesItemsQueryService;
        public OtherExpensesItemsGetAllHandler(IOtherExpensesItemsQueryService otherExpensesItemsQueryService)
        {
            _otherExpensesItemsQueryService = otherExpensesItemsQueryService;
            _otherExpensesItemsQueryService.NotNull(nameof(otherExpensesItemsQueryService));
        }

        public async Task<IEnumerable<OtherExpensesItemsGetDto>> Handle(CancellationToken cancellationToken)
        {
            IEnumerable<OtherExpensesItemsGetDto> data = await _otherExpensesItemsQueryService.Get();
            return data;
        }
    }
}
