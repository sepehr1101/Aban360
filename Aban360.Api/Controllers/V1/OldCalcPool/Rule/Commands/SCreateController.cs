using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/s")]
    public class SCreateController : BaseController
    {
        private readonly ISCreateHandler _sCreateHandler;
        public SCreateController(ISCreateHandler sCreateHandler)
        {
            _sCreateHandler = sCreateHandler;
            _sCreateHandler.NotNull(nameof(sCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(SCreateDto CreateDto, CancellationToken cancellationToken)
        {
            await _sCreateHandler.Handle(CreateDto, cancellationToken);
            return Ok(CreateDto);
        }
    }
}
