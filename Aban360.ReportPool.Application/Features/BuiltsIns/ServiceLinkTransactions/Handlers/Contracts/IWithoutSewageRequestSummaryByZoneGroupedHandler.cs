using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface IWithoutSewageRequestSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<WithoutSewageRequestSummaryHeaderOutputDto, ReportOutput<WithoutSewageRequestSummaryByZoneGroupedDataOutputDto, WithoutSewageRequestSummaryByZoneGroupedDataOutputDto>>> Handle(WithoutSewageRequestInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<WithoutSewageRequestSummaryHeaderOutputDto, WithoutSewageRequestSummaryByZoneGroupedDataOutputDto>> HandleFlat(WithoutSewageRequestInputDto input, CancellationToken cancellationToken);
    }
}