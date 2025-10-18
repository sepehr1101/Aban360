using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts
{
    public interface IEmptyUnitByBillHandler
    {
        Task<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>> Handle(EmptyUnitByBillInputDto input, CancellationToken cancellationToken);
    }
}
