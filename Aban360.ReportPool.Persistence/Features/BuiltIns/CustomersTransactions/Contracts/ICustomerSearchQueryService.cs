using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts
{
    public interface ICustomerSearchQueryService
    {
        Task<ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>> GetInfo(CustomerSearchInputDto input);
    }
}
