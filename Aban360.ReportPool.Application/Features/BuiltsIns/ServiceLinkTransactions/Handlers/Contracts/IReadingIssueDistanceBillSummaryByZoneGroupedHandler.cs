using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface IReadingIssueDistanceBillSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReportOutput<ReadingIssueDistanceBillSummryDataOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>>> Handle(ReadingIssueDistanceBillInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>> HandleFlat(ReadingIssueDistanceBillInputDto input, CancellationToken cancellationToken);
    }
}
