using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;

namespace Aban360.OldCalcPool.Persistence.Features.Processing.Queries.Contracts
{
    public interface ICustomerInfoDetailQueryService
    {
        Task<CustomerInfoOutputDto> GetInfo(string billId);
    }
}
