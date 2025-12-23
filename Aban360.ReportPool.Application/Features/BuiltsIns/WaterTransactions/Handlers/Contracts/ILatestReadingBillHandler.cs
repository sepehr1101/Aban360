using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;

namespace Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts
{
    public interface ILatestReadingBillHandler
    {
        Task<LatestReadingBillDataOutputDto> Handle(string billId, CancellationToken cancellationToken);
        Task<LatestReadingBillDataOutputDto> Handle_WithPreviousDb(string billId, CancellationToken cancellationToken);
    }
}
