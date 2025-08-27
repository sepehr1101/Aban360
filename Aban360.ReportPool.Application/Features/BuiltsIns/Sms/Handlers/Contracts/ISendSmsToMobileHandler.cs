using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.Sms.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.Sms.Handlers.Contracts
{
    public interface ISendSmsToMobileHandler
    {
        Task<ReportOutput<SendSmsToMobileHeaderOutputDto, SendSmsToMobileDataOutputDto>> Handle(SendSmsToMobileInputDto input, CancellationToken cancellationToken);
    }
}
