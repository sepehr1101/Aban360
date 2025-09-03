using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface IUseStateReportByZoneHandler
    {
        Task<ReportOutput<UseStateReportHeaderSummaryOutputDto, UseStateReportSummaryByZoneDataOutputDto>> Handle(UseStateReportInputDto input, CancellationToken cancellationToken);
    }
}
