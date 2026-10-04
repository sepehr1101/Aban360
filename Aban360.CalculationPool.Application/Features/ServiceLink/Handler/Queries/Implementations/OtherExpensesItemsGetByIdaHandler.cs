using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Implementations
{
    internal sealed class OtherExpensesItemsGetByIdaHandler : IOtherExpensesItemsGetByIdHandler
    {
        private readonly IOtherExpensesItemsQueryService _otherExpensesItemsQueryService;
        public OtherExpensesItemsGetByIdaHandler(IOtherExpensesItemsQueryService otherExpensesItemsQueryService)
        {
            _otherExpensesItemsQueryService = otherExpensesItemsQueryService;
            _otherExpensesItemsQueryService.NotNull(nameof(otherExpensesItemsQueryService));
        }

        public async Task<OtherExpensesItemsDataDto> Handle(int id, CancellationToken cancellationToken)
        {
            OtherExpensesItemsDataDto data = await _otherExpensesItemsQueryService.Get(id);
            return data;
        }
    }
}
