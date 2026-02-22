using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Commands.Contracts;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/meter-change")]
    public class MeterChangeController : BaseController
    {
        private readonly IMeterChangeCreateHandler _meterChangeCreateHandler;
        private readonly IMeterChangeGetHandler _meterChangeGetHandler;
        public MeterChangeController(
            IMeterChangeCreateHandler meterChangeCreateHandler,
            IMeterChangeGetHandler meterChangeGetHandler)
        {
            _meterChangeCreateHandler = meterChangeCreateHandler;
            _meterChangeCreateHandler.NotNull(nameof(meterChangeCreateHandler));

            _meterChangeGetHandler = meterChangeGetHandler;
            _meterChangeGetHandler.NotNull(nameof(meterChangeGetHandler));
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterChangeInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddMeterChange([FromBody] MeterChangeInputDto inputDto, CancellationToken cancellationToken)
        {
            await _meterChangeCreateHandler.Handle(inputDto, cancellationToken);
            return Ok(inputDto);
        }


        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterChangeInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeterChange([FromBody] SearchInput inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<MeterChangeInfoOutputDto> result = await _meterChangeGetHandler.Handle(inputDto.Input, cancellationToken);
            return Ok(result);
        }
    }
}
