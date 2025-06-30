using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.Sms.Contracts
{
    public interface IGetCustomerMobileQueryService
    {
        Task<GetCustomerMobileOutputDto> Get(GetCustomerMobileInputDto input);
    }
}
