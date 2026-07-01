using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;

namespace Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Implementations
{
    internal sealed class MeterReadingDownloadExcelFileHandler : IMeterReadingDownloadExcelFileHandler
    {
        private readonly IBillQueryService _billQueryService;
        private readonly ICommonZoneService _commonZoneService;
        private string _title = ReportLiterals.WaterLatestList;
        public MeterReadingDownloadExcelFileHandler(
            IBillQueryService billQueryService,
            ICommonZoneService commonZoneService)
        {
            _billQueryService = billQueryService;
            _billQueryService.NotNull(nameof(billQueryService));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));
        }

        public async Task<ReportOutput<MeterReadingExcelFileDownloadHeaderOutputDto, MeterReadingExcelFileDownloadDateOutputDto>> Handle(MeterReadingExcelFileDownloadDto input, CancellationToken cancellationToken)
        {
            IEnumerable<MeterReadingExcelFileDownloadDateOutputDto> data = await _billQueryService.GetLatestListForExcel(new BillLatestListInputDto(input.ZoneId, input.FromReadingNumber, input.ToReadingNumber));
            MeterReadingExcelFileDownloadHeaderOutputDto header = new()
            {
                ZoneId = input.ZoneId,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = data?.Count() ?? 0,
                CustomerCount = data?.Count() ?? 0,
                Title = _title,
            };

            return new ReportOutput<MeterReadingExcelFileDownloadHeaderOutputDto, MeterReadingExcelFileDownloadDateOutputDto>(_title, header, data);
        }
    }
}