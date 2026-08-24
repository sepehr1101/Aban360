using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Features.Bill.Queries.Contracts;
using Aban360.Common.ApplicationUser;
using Aban360.Common.Extensions;

namespace Aban360.CalculationPool.Application.Features.CollectBills.Handlers.Commands.Implementations
{
    internal sealed class CollectBillsFileCreateHandler : ICollectBillsFileCreateHandler
    {
        private readonly ICollectBillsQueryService _collectBillsQueryService;
        private readonly ICollectBillsDetailJobService _collectBillsDetailJobService;
        public CollectBillsFileCreateHandler(
            ICollectBillsQueryService collectBillsQueryService,
            ICollectBillsDetailJobService collectBillsDetailJobService)
        {
            _collectBillsQueryService = collectBillsQueryService;
            _collectBillsQueryService.NotNull(nameof(collectBillsQueryService));

            _collectBillsDetailJobService = collectBillsDetailJobService;
            _collectBillsDetailJobService.NotNull(nameof(collectBillsDetailJobService));
        }

        public async Task<CollectBillsGetZipFileInfo> Handle(string? reportDateJalali, IAppUser appUser, CancellationToken cancellationToken)
        {
            CollectBillsGetDataToSendInputDto dtoToGenerateTxtFile = new(fromDateJalali: reportDateJalali, toDateJalali: reportDateJalali);
            IEnumerable<CollectBillsDataDto> data = await _collectBillsQueryService.Get(dtoToGenerateTxtFile);
            CollectBillsGetZipFileInfo zipFileInfo = await _collectBillsDetailJobService.CreateZip(data.Select(s => s.Row).ToList(), dtoToGenerateTxtFile.FromDateJalali, dtoToGenerateTxtFile.FromDateJalali);

            return zipFileInfo;
        }
    }
}
