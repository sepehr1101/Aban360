using Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries;

namespace Aban360.MeterPool.Persistence.Features.Reading.Queries.Contracts
{
    public interface IReadingCustomerDetailInfoQueryService
    {
        Task<IEnumerable<CustomerInfoByBillsAndChangeGetDto>> Get(int zoneId, string fromReadingNumber, string toReadingNumber);
    }
}
