using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.WaterReturn.Queries.Contracts
{
    public interface IAutoBackQueryService
    {
        Task<AutoBackGetDto> Get(ReturnBillConfirmeByZoneAndCustomerNumberInputDto input);
    }
}