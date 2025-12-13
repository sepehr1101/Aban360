using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Implementations
{
    internal sealed class LatestReadingBillHandler : ILatestReadingBillHandler
    {
        private readonly ILatestReadingBillQueryService _latestReadingBillQueryService;
        public LatestReadingBillHandler(
            ILatestReadingBillQueryService latestReadingBillQueryService)
        {
            _latestReadingBillQueryService = latestReadingBillQueryService;
            _latestReadingBillQueryService.NotNull(nameof(latestReadingBillQueryService));
        }

        public async Task<LatestReadingBillDataOutputDto> Handle(string billId, CancellationToken cancellationToken)
        {
            LatestReadingBillDataOutputDto LatestReadingBill = await _latestReadingBillQueryService.GetInfo(billId);
            return LatestReadingBill;
        }
    }
}
