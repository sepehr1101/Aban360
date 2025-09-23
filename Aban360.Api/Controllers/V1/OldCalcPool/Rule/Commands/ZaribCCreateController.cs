using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/zarib-c")]
    public class ZaribCCreateController : BaseController
    {
        private readonly IZaribCCreateHandler _zaribCCreateHandler;
        public ZaribCCreateController(IZaribCCreateHandler zaribCCreateHandler)
        {
            _zaribCCreateHandler = zaribCCreateHandler;
            _zaribCCreateHandler.NotNull(nameof(zaribCCreateHandler));
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(ZaribCCreateDto CreateDto, CancellationToken cancellationToken)
        {
            await _zaribCCreateHandler.Handle(CreateDto, cancellationToken);
            return Ok(CreateDto);
        }
    }
}
