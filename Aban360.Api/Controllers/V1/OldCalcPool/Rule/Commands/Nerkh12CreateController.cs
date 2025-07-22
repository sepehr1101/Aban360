using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/nerkh-12")]
    public class Nerkh12CreateController : BaseController
    {
        private readonly INerkhCreateHandler _nerkhCreateHandler;
        public Nerkh12CreateController(INerkhCreateHandler nerkhCreateHandler)
        {
            _nerkhCreateHandler = nerkhCreateHandler;
            _nerkhCreateHandler.NotNull(nameof(nerkhCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NerkhCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create( NerkhCreateDto createDto, CancellationToken cancellationToken)
        {
            await _nerkhCreateHandler.Handle(createDto, 12, cancellationToken);
            return Ok(createDto);
        }
    }
}
