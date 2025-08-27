using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts
{
    public interface ICustomerSearchAdvancedHandler
    {
        Task<ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>> Handle(CustomerSearchAdvancedInputDto input, CancellationToken cancellationToken);
    }
}
