using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Cronjobs;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Services;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.CalculationPool.Application.Features.Base;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading
{
    public class MeterReadingFileController : BaseController
    {
        private readonly IChangeDateBatchHandler _c0_changeDateBatchHandler;
        private readonly IMeterReadingExcelFileCreateHandler _c1_meterReadingFileHandle;
        private readonly IMeterReadingExcelMultiZoneFileCreateHandler _c1_meterReadingMultiZoneFileHandler;
        private readonly IMeterReadingDownloadExcelFileHandler _c1_meterReadingDownloadExcelFile;
        private readonly ICommonZoneService _c1_commonZoneService;
        private readonly IReportGenerator _c1_reportGenerator;
        private readonly IMeterReadingFileCreateHandler _c2_meterReadingFileHandle;
        private readonly IMeterReadingUploadDbFileWithSendCloseSmsJobService _c2_meterReadingUploadDbFileWithSendSmsJobService;
        public MeterReadingFileController(IChangeDateBatchHandler c0_changeDateBatchHandler, IMeterReadingExcelFileCreateHandler c1_meterReadingFileHandle, IMeterReadingExcelMultiZoneFileCreateHandler c1_meterReadingMultiZoneFileHandler, IMeterReadingDownloadExcelFileHandler c1_meterReadingDownloadExcelFile, ICommonZoneService c1_commonZoneService, IReportGenerator c1_reportGenerator, IMeterReadingFileCreateHandler c2_meterReadingFileHandle, IMeterReadingUploadDbFileWithSendCloseSmsJobService c2_meterReadingUploadDbFileWithSendSmsJobService)
        {
            _c0_changeDateBatchHandler = c0_changeDateBatchHandler;
            _c0_changeDateBatchHandler.NotNull(nameof(c0_changeDateBatchHandler));
            _c1_meterReadingFileHandle = c1_meterReadingFileHandle;
            _c1_meterReadingFileHandle.NotNull(nameof(c1_meterReadingFileHandle));
            _c1_meterReadingMultiZoneFileHandler = c1_meterReadingMultiZoneFileHandler;
            _c1_meterReadingMultiZoneFileHandler.NotNull(nameof(c1_meterReadingMultiZoneFileHandler));
            _c1_meterReadingDownloadExcelFile = c1_meterReadingDownloadExcelFile;
            _c1_meterReadingDownloadExcelFile.NotNull(nameof(c1_meterReadingDownloadExcelFile));
            _c1_commonZoneService = c1_commonZoneService;
            _c1_commonZoneService.NotNull(nameof(c1_commonZoneService));
            _c1_reportGenerator = c1_reportGenerator;
            _c1_reportGenerator.NotNull(nameof(c1_reportGenerator));
            _c2_meterReadingFileHandle = c2_meterReadingFileHandle;
            _c2_meterReadingFileHandle.NotNull(nameof(c2_meterReadingFileHandle));
            _c2_meterReadingUploadDbFileWithSendSmsJobService = c2_meterReadingUploadDbFileWithSendSmsJobService;
            _c2_meterReadingUploadDbFileWithSendSmsJobService.NotNull(nameof(c2_meterReadingUploadDbFileWithSendSmsJobService));
        }

        [HttpPost]
        [Route("~/v1/meter-reading-file/change-date-batch")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ChangeDateBatchOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeDate([FromBody] ChangeDateBatchInputDto input, CancellationToken cancellationToken)
        {
            ChangeDateBatchOutputDto result = await _c0_changeDateBatchHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/meter-reading-file/upload-excel")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>>), StatusCodes.Status200OK)]
        [ActionName("Upload")]
        public async Task<IActionResult> UploadFromMeterReadingExcelFile([FromForm] MeterReadingExcelFileCreateDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = await _c1_meterReadingFileHandle.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/meter-reading-file/upload-multi-zone-excel")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadMultiZone([FromForm] MeterReadingExcelFileCreateDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = await _c1_meterReadingMultiZoneFileHandler.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/meter-reading-file/download-excel/{connectionId}")]
        public async Task<IActionResult> Download(string connectionId, [FromBody] MeterReadingExcelFileDownloadDto input, CancellationToken cancellationToken)
        {
            await _c1_commonZoneService.IsUserInZone(CurrentUser, input.ZoneId);
            ReportOutput<MeterReadingExcelFileDownloadHeaderOutputDto, MeterReadingExcelFileDownloadDateOutputDto> result = await _c1_meterReadingDownloadExcelFile.Handle(input, cancellationToken);
            await _c1_reportGenerator.FireAndInform(input, cancellationToken, _c1_meterReadingDownloadExcelFile.Handle, CurrentUser, ReportLiterals.MeterReadingExcelFile, connectionId);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/meter-reading-file/download-multi-zone-excel/{connectionId}")]
        public async Task<IActionResult> DownloadMultiZone(string connectionId, [FromBody] MeterReadingExcelFileDownloadDto input, CancellationToken cancellationToken)
        {
            await _c1_commonZoneService.IsUserInZone(CurrentUser, input.ZoneId);
            ReportOutput<MeterReadingExcelFileDownloadHeaderOutputDto, MeterReadingExcelFileDownloadDateOutputDto> result = await _c1_meterReadingDownloadExcelFile.Handle(input, cancellationToken);
            await _c1_reportGenerator.FireAndInform(input, cancellationToken, _c1_meterReadingDownloadExcelFile.Handle, CurrentUser, ReportLiterals.MeterReadingExcelFile, connectionId);
            return Ok(result);
        }

        [HttpPost]
        [Route("~/v1/meter-reading-file/upload")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingFileCreateOutputDto>), StatusCodes.Status200OK)]
        [ActionName("Upload")]
        public async Task<IActionResult> UploadFromMeterReadingFileUpload([FromForm] MeterReadingFileCreateDto input, CancellationToken cancellationToken)
        {
            //MeterReadingFileCreateOutputDto result = await _meterReadingFileHandle.Handle(input, CurrentUser, cancellationToken);
            //return Ok(result);
            await _c2_meterReadingUploadDbFileWithSendSmsJobService.Upload(input, CurrentUser, cancellationToken);
            return Ok(input);
        }
    }
}

