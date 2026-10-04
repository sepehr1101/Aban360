using Aban360.CalculationPool.Domain.Features.ServiceLink;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts
{
    public interface IOtherExpensesItemsGetAllHandler
    {
        Task<IEnumerable<OtherExpensesItemsDataDto>> Handle(CancellationToken cancellationToken);
    }
}
