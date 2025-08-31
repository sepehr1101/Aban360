using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.Sms.Contracts
{
    public interface ISendSmsToMobileQueryService
    {
        Task<ReportOutput< SendSmsToMobileHeaderOutputDto, SendSmsToMobileDataOutputDto>> Get(SendSmsToMobileInputDto input);
    }
}
