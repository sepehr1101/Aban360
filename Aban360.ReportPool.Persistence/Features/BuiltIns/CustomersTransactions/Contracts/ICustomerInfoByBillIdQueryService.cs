using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts
{
    public interface ICustomerInfoByBillIdQueryService
    {
        Task<CustomerInfoByBillIdOutputDto> Get(string billId);
    }
}
