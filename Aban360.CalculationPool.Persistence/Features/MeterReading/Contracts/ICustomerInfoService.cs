using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts
{
    public interface ICustomerInfoService
    {
        Task<CustomerInfoGetDto> Get(int zoneId, int customerNumber);
    }
}
