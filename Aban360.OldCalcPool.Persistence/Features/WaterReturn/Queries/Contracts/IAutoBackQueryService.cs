using Aban360.Common.BaseEntities;
using Aban360.OldCalcPool.Domain.Features.WaterReturn.Dto.Queries;

namespace Aban360.OldCalcPool.Persistence.Features.WaterReturn.Queries.Contracts
{
    public interface IAutoBackQueryService
    {
        Task<AutoBackGetDto> Get(ReturnBillConfirmeByZoneAndCustomerNumberInputDto input);
        Task<IEnumerable<AutoBackGetByBargeDto>> GetByConfirmNumber(int confirmedNumber);
        Task<IEnumerable<UnconfirmedBillReturnDataOutputDto>> GetUnconfirmed(int zoneId);
        Task<IEnumerable<UnconfirmedBillReturnDataOutputDto>> GetUnconfirmed(ZoneIdAndCustomerNumber input,string billId);
        Task<int> GetCountByDateInterval(ReturnBillDateIntervalDto input);
    }
}