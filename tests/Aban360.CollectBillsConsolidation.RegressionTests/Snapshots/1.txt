using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Db.Constants.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.CollectBills.Services
{
    public interface ICollectBillsFilesService
    {
        CollectBillsGetZipFileInfo CollectBillsDetailFileDownloadAsync(string fileName, CancellationToken cancellationToken);
        Task<CollectBillsGetZipFileInfo> CollectBillsFileCreateAsync(string? reportDateJalali, IAppUser appUser, CancellationToken cancellationToken);
    }

    internal sealed class CollectBillsFilesService : ICollectBillsFilesService
    {
        private string _CollectBillsDetailFileDownloadfolderPath = DirectoryLiterals.CollectBillsFolderPath;
        private readonly ICollectBillsQueryService _collectBillsQueryService;
        private readonly ICollectBillsDetailJobService _collectBillsDetailJobService;
        public CollectBillsFilesService(ICollectBillsQueryService collectBillsQueryService, ICollectBillsDetailJobService collectBillsDetailJobService)
        {
            _collectBillsQueryService = collectBillsQueryService;
            _collectBillsQueryService.NotNull(nameof(collectBillsQueryService));
            _collectBillsDetailJobService = collectBillsDetailJobService;
            _collectBillsDetailJobService.NotNull(nameof(collectBillsDetailJobService));
        }

        public CollectBillsGetZipFileInfo CollectBillsDetailFileDownloadAsync(string fileName, CancellationToken cancellationToken)
        {
            string filePath = Path.Combine(_CollectBillsDetailFileDownloadfolderPath, fileName);
            CollectBillsGetZipFileInfo result = new(filePath, fileName);
            return result;
        }

        public async Task<CollectBillsGetZipFileInfo> CollectBillsFileCreateAsync(string? reportDateJalali, IAppUser appUser, CancellationToken cancellationToken)
        {
            CollectBillsGetDataToSendInputDto dtoToGenerateTxtFile = new(fromDateJalali: reportDateJalali, toDateJalali: reportDateJalali);
            IEnumerable<CollectBillsDataDto> data = await _collectBillsQueryService.Get(dtoToGenerateTxtFile);
            CollectBillsGetZipFileInfo zipFileInfo = await _collectBillsDetailJobService.CreateZip(data.Select(s => s.Row).ToList(), dtoToGenerateTxtFile.FromDateJalali, dtoToGenerateTxtFile.FromDateJalali);
            return zipFileInfo;
        }
    }
}
