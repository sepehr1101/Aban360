using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts
{
    public interface IUsageHandler
    {
        Task<ReportOutput<UsageHeaderOutputDto, UsageDataOutputDto>> Handle(UsageInputDto input, CancellationToken cancellationToken);
    }
}
