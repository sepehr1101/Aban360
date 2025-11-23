using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/meter-reading-file")]
    public class MeterReadingFileCreateController : BaseController
    {
        static string _pathBase = "AppData\\Dbfs";
        private readonly IMeterReadingFileCreateHandler _meterReadingFileHandle;
        private readonly IMeterFlowValidationGetHandler _meterFlowValidationGetHandler;
        public MeterReadingFileCreateController(
            IMeterReadingFileCreateHandler meterReadingFileHandle,
            IMeterFlowValidationGetHandler meterFlowValidationGetHandler)
        {
            _meterReadingFileHandle = meterReadingFileHandle;
            _meterReadingFileHandle.NotNull(nameof(meterReadingFileHandle));

            _meterFlowValidationGetHandler = meterFlowValidationGetHandler;
            _meterFlowValidationGetHandler.NotNull(nameof(meterFlowValidationGetHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterReadingFileCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(MeterReadingFileCreateDto input, CancellationToken cancellationToken)
        {
            await _meterFlowValidationGetHandler.Handle(input.ReadingFile.FileName, cancellationToken);
            input.FilePath = await CopyDbfFileInDbfs(input.ReadingFile);
            await _meterReadingFileHandle.Handle(input, CurrentUser, cancellationToken);

            return Ok(input);
        }
        private async Task<string> CopyDbfFileInDbfs(IFormFile file)
        {
            string filePath = Path.Combine(_pathBase, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}
