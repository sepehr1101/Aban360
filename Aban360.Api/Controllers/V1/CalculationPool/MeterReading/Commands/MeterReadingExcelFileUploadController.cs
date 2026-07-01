using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-reading-file")]
    public class MeterReadingExcelFileUploadController : BaseController
    {
        private readonly IMeterReadingExcelFileCreateHandler _meterReadingFileHandle;

        public MeterReadingExcelFileUploadController(IMeterReadingExcelFileCreateHandler meterReadingFileHandle)
        {
            _meterReadingFileHandle = meterReadingFileHandle;
            _meterReadingFileHandle.NotNull(nameof(meterReadingFileHandle));
        }

        [HttpPost]
        [Route("upload-excel")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload([FromForm] MeterReadingExcelFileCreateDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = await _meterReadingFileHandle.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
