using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.Sms.Handlers.Contracts
{
    public interface IGetCustomerMobileHandler
    {
        Task<GetCustomerMobileOutputDto> Handle(GetCustomerMobileInputDto input, CancellationToken cancellationToken);
    }
}
