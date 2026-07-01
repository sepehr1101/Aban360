using Aban360.Api.Cronjobs;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Db.Services;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-reading-file")]
    public class MeterReadingExcelFileController : BaseController
    {
        private readonly IMeterReadingExcelFileCreateHandler _meterReadingFileHandle;
        private readonly IMeterReadingDownloadExcelFileHandler _meterReadingDownloadExcelFile;
        private readonly ICommonZoneService _commonZoneService;
        private readonly IReportGenerator _reportGenerator;
        public MeterReadingExcelFileController(
            IMeterReadingExcelFileCreateHandler meterReadingFileHandle,
            IMeterReadingDownloadExcelFileHandler meterReadingDownloadExcelFile,
            ICommonZoneService commonZoneService,
            IReportGenerator reportGenerator)
        {
            _meterReadingFileHandle = meterReadingFileHandle;
            _meterReadingFileHandle.NotNull(nameof(meterReadingFileHandle));

            _meterReadingDownloadExcelFile = meterReadingDownloadExcelFile;
            _meterReadingDownloadExcelFile.NotNull(nameof(meterReadingDownloadExcelFile));

            _commonZoneService = commonZoneService;
            _commonZoneService.NotNull(nameof(commonZoneService));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("upload-excel")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload([FromForm] MeterReadingExcelFileCreateDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = await _meterReadingFileHandle.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("download-excel/{connectionId}")]
        public async Task<IActionResult> Download(string connectionId, [FromBody] MeterReadingExcelFileDownloadDto input, CancellationToken cancellationToken)
        {
            await _commonZoneService.IsUserInZone(CurrentUser, input.ZoneId);
            ReportOutput<MeterReadingExcelFileDownloadHeaderOutputDto, MeterReadingExcelFileDownloadDateOutputDto> result = await _meterReadingDownloadExcelFile.Handle(input, cancellationToken);
            await _reportGenerator.FireAndInform(input, cancellationToken, _meterReadingDownloadExcelFile.Handle, CurrentUser, ReportLiterals.MeterReadingExcelFile, connectionId);
            return Ok(result);
        }
    }
}
