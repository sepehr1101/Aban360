using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface IServiceLinkRawItemsDetailHandler
    {
        Task<ReportOutput<ServiceLinkRawItemsHeaderOutputDto, ServiceLinkRawItemsDetailDataOutputDto>> Handle(ServiceLinkRawItemsInputDto input, CancellationToken cancellationToken);
    }
}
