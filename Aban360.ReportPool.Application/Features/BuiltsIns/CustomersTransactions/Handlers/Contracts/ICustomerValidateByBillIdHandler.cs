using Aban360.Common.ApplicationUser;
using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts
{
    public interface ICustomerValidateByBillIdHandler
    {
        Task<CustomerBillIdValidateDto> Handle(SearchInput input, IAppUser appUser, CancellationToken cancellationToken);
    }
}
