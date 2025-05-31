using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts
{
    public interface ICustomerSearchAdvancedHandler
    {
        Task<ICollection<CustomerSearchOutputDto>> Handle(CustomerSearchAdvancedInputDto input, CancellationToken cancellationToken);
    }
}
