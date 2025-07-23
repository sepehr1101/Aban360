using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/zarib")]
    public class ZaribCreateController : BaseController
    {
        private readonly IZaribCreateHandler _zaribCreateHandler;
        public ZaribCreateController(IZaribCreateHandler zaribCreateHandler)
        {
            _zaribCreateHandler = zaribCreateHandler;
            _zaribCreateHandler.NotNull(nameof(zaribCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(ZaribCreateDto createDto, CancellationToken cancellationToken)
        {
            await _zaribCreateHandler.Handle(createDto, cancellationToken);
            return Ok(createDto);
        }
    }
}
