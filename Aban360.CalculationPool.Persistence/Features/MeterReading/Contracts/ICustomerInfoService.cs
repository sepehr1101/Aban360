using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.BaseEntities;

namespace Aban360.CalculationPool.Persistence.Features.MeterReading.Contracts
{
    public interface ICustomerInfoService
    {
        //Task<ZoneIdAndCustomerNumberGetDto> GetZoneIdAndCustomerNumber(string billId);
        Task<CustomerInfoGetDto> Get(int zoneId, int customerNumber);
        Task<CustomersInfoGetDto> Get(int zoneId, ICollection<int> customerNumbers);
        Task<CustomerGeneralInfoGetDto> Get(string billId);
        Task<double> GetMembersBedBes(ZoneIdAndCustomerNumber input);
    }
}
