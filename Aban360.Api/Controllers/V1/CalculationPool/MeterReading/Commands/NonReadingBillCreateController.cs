using Aban360.CalculationPool.Application.Features.MeterReading.Handlers.Commands.Creata.Contracts;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.MeterReading.Commands
{
    [Route("v1/non-read")]
    public class NonReadingBillCreateController : BaseController
    {
        private readonly IMeterReadingNonReadCreateHandler _meterReadingNonReadHandle;
        public NonReadingBillCreateController(IMeterReadingNonReadCreateHandler meterReadingNonReadHandle)
        {
            _meterReadingNonReadHandle = meterReadingNonReadHandle;
            _meterReadingNonReadHandle.NotNull(nameof(meterReadingNonReadHandle));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload([FromBody] MeterReadingNonReadInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<MeterReadingDetailHeaderOutputDto, MeterReadingDetailCreateDto> result = await _meterReadingNonReadHandle.Handle(input, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
