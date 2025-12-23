using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class LatestReadingBillHandler : ILatestReadingBillHandler
    {
        private readonly ILatestReadingBillQueryService _latestReadingBillQueryService;
        private readonly ICustomerInfoQueryService _customerInfoQueryService;
        public LatestReadingBillHandler(
            ILatestReadingBillQueryService latestReadingBillQueryService,
            ICustomerInfoQueryService customerInfoQueryService)
        {
            _latestReadingBillQueryService = latestReadingBillQueryService;
            _latestReadingBillQueryService.NotNull(nameof(latestReadingBillQueryService));

            _customerInfoQueryService = customerInfoQueryService;
            _customerInfoQueryService.NotNull(nameof(customerInfoQueryService));
        }

        public async Task<LatestReadingBillDataOutputDto> Handle(string billId, CancellationToken cancellationToken)
        {
            LatestReadingBillDataOutputDto LatestReadingBill = await _latestReadingBillQueryService.GetInfo(billId);
            return LatestReadingBill;
        }
        public async Task<LatestReadingBillDataOutputDto> Handle_WithPreviousDb(string billId, CancellationToken cancellationToken)
        {
            ZoneIdAndCustomerNumberOutputDto zoneIdAndCustomerNumber=await _customerInfoQueryService.GetZoneIdAndCustomerNumber(billId);
            LatestReadingBillDataOutputDto LatestReadingBill = await _latestReadingBillQueryService.GetInfo(zoneIdAndCustomerNumber);
            return LatestReadingBill;
        }
    }
}
