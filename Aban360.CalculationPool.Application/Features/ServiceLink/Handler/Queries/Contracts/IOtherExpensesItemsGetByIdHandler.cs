using Aban360.CalculationPool.Domain.Features.ServiceLink;

namespace Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts
{
    public interface IOtherExpensesItemsGetByIdHandler
    {
        Task<OtherExpensesItemsGetDto> Handle(int id, CancellationToken cancellationToken);
    }
}
