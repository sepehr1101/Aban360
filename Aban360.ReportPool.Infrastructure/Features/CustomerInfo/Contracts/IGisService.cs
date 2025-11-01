using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;

namespace Aban360.ReportPool.Infrastructure.Features.CustomerInfo.Contracts
{
    public interface IGisService
    {
        Task<CustomerLocationDto> GetCustomerLocation(CustomerLocationInputDto inputDto);
    }
}
