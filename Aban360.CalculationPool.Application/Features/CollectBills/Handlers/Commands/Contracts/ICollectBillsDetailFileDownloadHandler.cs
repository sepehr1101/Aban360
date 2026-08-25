using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;

namespace Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Commands.Contracts
{
    public interface ICollectBillsDetailFileDownloadHandler
    {
        CollectBillsGetZipFileInfo Handle(string fileName, CancellationToken cancellationToken);
    }
}
