using Aban360.CalculationPool.Application.Features.Base;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-reading-file")]
    public class MeterReadingFileUploadController : BaseController
    {
        private readonly IMeterReadingFileCreateHandler _meterReadingFileHandle;
        private readonly IMeterReadingUploadDbFileWithSendSmsJobService _meterReadingUploadDbFileWithSendSmsJobService;

        public MeterReadingFileUploadController(
            IMeterReadingFileCreateHandler meterReadingFileHandle,
            IMeterReadingUploadDbFileWithSendSmsJobService meterReadingUploadDbFileWithSendSmsJobService)
        {
            _meterReadingFileHandle = meterReadingFileHandle;
            _meterReadingFileHandle.NotNull(nameof(meterReadingFileHandle));

            _meterReadingUploadDbFileWithSendSmsJobService = meterReadingUploadDbFileWithSendSmsJobService;
            _meterReadingUploadDbFileWithSendSmsJobService.NotNull(nameof(meterReadingUploadDbFileWithSendSmsJobService));
        }

        [HttpPost]
        [Route("upload")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload([FromForm] MeterReadingFileCreateDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = await _meterReadingFileHandle.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
            //await _meterReadingUploadDbFileWithSendSmsJobService.Upload(input, CurrentUser, cancellationToken);
            //return Ok(input);
        }
    }
}
