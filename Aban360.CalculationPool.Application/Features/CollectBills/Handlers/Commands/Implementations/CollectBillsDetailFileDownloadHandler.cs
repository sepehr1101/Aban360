using Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Db.Constants.Literals;
using Aban360.ReportPool.Domain.Base;

namespace Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Commands.Implementations
{
    internal sealed class CollectBillsDetailFileDownloadHandler : ICollectBillsDetailFileDownloadHandler
    {
        private string _folderPath = DirectoryLiterals.CollectBillsFolderPath;
        public CollectBillsGetZipFileInfo Handle(string fileName, CancellationToken cancellationToken)
        {
            string filePath = Path.Combine(_folderPath, fileName);
            CollectBillsGetZipFileInfo result = new(filePath, fileName);
            
            return result;
        }
    }
}
