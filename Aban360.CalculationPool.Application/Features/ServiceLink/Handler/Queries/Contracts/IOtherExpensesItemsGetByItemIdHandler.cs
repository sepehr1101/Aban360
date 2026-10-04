using Aban360.CalculationPool.Domain.Features.ServiceLink;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts
{
    public interface IOtherExpensesItemsGetByItemIdHandler
    {
        Task<OtherExpensesItemsDataDto> Handle(string billId, int itemId, CancellationToken cancellationToken);
    }
}
