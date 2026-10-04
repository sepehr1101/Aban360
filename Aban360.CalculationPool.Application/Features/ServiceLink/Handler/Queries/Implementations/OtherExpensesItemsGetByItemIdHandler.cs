using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Implementations
{
    internal sealed class OtherExpensesItemsGetByItemIdHandler : IOtherExpensesItemsGetByItemIdHandler
    {
        private readonly IOtherExpensesItemsQueryService _otherExpensesItemsQueryService;
        public OtherExpensesItemsGetByItemIdHandler(IOtherExpensesItemsQueryService otherExpensesItemsQueryService)
        {
            _otherExpensesItemsQueryService = otherExpensesItemsQueryService;
            _otherExpensesItemsQueryService.NotNull(nameof(otherExpensesItemsQueryService));
        }

        public async Task<OtherExpensesItemsGetDto> Handle(int id, CancellationToken cancellationToken)
        {
            //OtherExpensesItemsGetDto data = await _otherExpensesItemsQueryService.GetByItemId(id);
            OtherExpensesItemsGetDto data = new()
            {
                Id = 1,
                ItemId = 1,
                ItemTitle = string.Empty,
                Amount = 1,
                InsertDateTime = DateTime.Now,
                RemoveDateTime = null,
            };
            return data;
        }
    }
}
