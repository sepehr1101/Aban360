using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.WaterReturn.Queries.Contracts
{
    public interface IAutoBackQueryService
    {
        Task<AutoBackGetDto> Get(ReturnBillConfirmeByZoneAndCustomerNumberInputDto input);
        Task<IEnumerable<AutoBackGetByBargeDto>> GetByConfirmNumber(int confirmedNumber);
        Task<IEnumerable<UnconfirmedBillReturnDataOutputDto>> GetUnconfirmed(int zoneId);
        Task<int> GetCountByDateInterval(ReturnBillDateIntervalDto input);
    }
}