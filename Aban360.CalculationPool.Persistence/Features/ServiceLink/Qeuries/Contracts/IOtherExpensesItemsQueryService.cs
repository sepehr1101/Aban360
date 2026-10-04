using Aban360.CalculationPool.Domain.Features.ServiceLink;

namespace Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts
{
    public interface IOtherExpensesItemsQueryService
    {
        Task<IEnumerable<OtherExpensesItemsGetDto>> Get();
        Task<OtherExpensesItemsGetDto> Get(int id);
        Task<OtherExpensesItemsGetDto> GetByItemId(int id);
    }
}
