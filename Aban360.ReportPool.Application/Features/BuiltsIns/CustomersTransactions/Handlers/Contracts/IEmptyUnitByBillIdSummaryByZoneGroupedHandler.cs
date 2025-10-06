using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using System.Runtime.InteropServices;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts
{
    public interface IEmptyUnitByBillIdSummaryByZoneGroupedHandler
    {
        Task<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, ReportOutput<EmptyUnitByBillIdByZoneGroupedDataOutputDto, EmptyUnitByBillIdByZoneGroupedDataOutputDto>>> Handle(EmptyUnitInputDto input, CancellationToken cancellationToken);
        Task<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdByZoneGroupedDataOutputDto>> HandleFlat(EmptyUnitInputDto input, [Optional] CancellationToken cancellationToken);
    }
}
