using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface IUseStateReportSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<UseStateReportHeaderSummaryOutputDto, ReportOutput<UseStateReportSummaryByZoneGroupedDataOutputDto, UseStateReportSummaryByZoneGroupedDataOutputDto>>> Handle(UseStateReportInputDto input, CancellationToken cancellationToken);
    }
}
