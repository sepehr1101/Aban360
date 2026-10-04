using Aban360.CalculationPool.Domain.Features.ServiceLink;

namespace Aban360.CalculationPool.Persistence.Features.ServiceLink.Qeuries.Contracts
{
    public interface IOtherExpensesItemsQueryService
    {
        Task<IEnumerable<OtherExpensesItemsDataDto>> Get();
        Task<OtherExpensesItemsDataDto> Get(int id);
        Task<OtherExpensesItemsDataDto> Get(OtherExpensesItemsGetDto inputDto);
    }
}
