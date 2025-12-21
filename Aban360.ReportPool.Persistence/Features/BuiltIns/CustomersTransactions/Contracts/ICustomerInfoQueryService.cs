using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.Transactions;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts
{
    public interface ICustomerInfoQueryService
    {
        Task<CustomerInfoByBillIdOutputDto> Get(string billId);
        Task<BillIdReppar> Get(CustomerInfoByZoneAndCustomerNumberInputDto input);
        Task<ZoneIdAndCustomerNumberOutputDto> GetZoneIdAndCustomerNumber(string billId);
    }
}
