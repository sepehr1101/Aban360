using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.ClaimPool.Domain.Features.Request.Dto.Queries;

namespace Aban360.ClaimPool.Persistence.Features.Request.Queries.Contracts
{
    public interface IKartQueryService
    {
        Task<IEnumerable<CalculationRequestDisplayDataOutputDto>> Get(string stringTrackNumber, int zoneId);
        Task<CalculationRequestDisplayDataOutputDto> Get(int id, int zoneId);
        Task<IEnumerable<KartGetDto>> GetAll(string stringTrackNumber, int zoneId);
        Task<IEnumerable<KartGetDto>> GetAll(int customerNumber, int zoneId);
        Task<IEnumerable<ServiceLinkUnconfirmedDataOutputDto>> GetTodayInfoByCustomerNumber(int customerNumber, int zoneId);
    }
}
