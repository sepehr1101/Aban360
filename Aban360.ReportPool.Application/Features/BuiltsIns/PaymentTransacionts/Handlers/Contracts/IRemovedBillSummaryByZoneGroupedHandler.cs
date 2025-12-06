using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts
{
    public interface IRemovedBillSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<RemovedBillHeaderOutputDto, ReportOutput<RemovedBillSummaryDataOutputDto, RemovedBillSummaryDataOutputDto>>> Handle(RemovedBillRawInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<RemovedBillHeaderOutputDto, RemovedBillSummaryDataOutputDto>> HandleFlat(RemovedBillRawInputDto input,  CancellationToken cancellationToken);


    }
}
