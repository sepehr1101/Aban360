using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts
{
    public interface IMeterDuplicateChangeWithCustomerDetailHandler
    {
        Task<IEnumerable<MeterDuplicateChangeWithCustomerDetailDataOutputDto>> Handle(MeterDuplicateChangeWithCustomerInputDto input, CancellationToken cancellationToken);
    }
}
