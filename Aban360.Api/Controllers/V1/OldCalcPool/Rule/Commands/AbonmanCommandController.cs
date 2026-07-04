using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/abonman")]
    public class AbonmanCommandController : BaseController
    {
        private readonly IAbonmanCreateHandler _abonmanCreateHandler;
        private readonly IAbonmanUpdateHandler _abonmanUpdateHandler;
        public AbonmanCommandController(
            IAbonmanCreateHandler abonmanCreateHandler,
            IAbonmanUpdateHandler abonmanUpdateHandler)
        {
            _abonmanCreateHandler = abonmanCreateHandler;
            _abonmanCreateHandler.NotNull(nameof(abonmanCreateHandler));

            _abonmanUpdateHandler = abonmanUpdateHandler;
            _abonmanUpdateHandler.NotNull(nameof(abonmanUpdateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbonmanCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(AbonmanCreateDto inputDto, CancellationToken cancellationToken)
        {
            await _abonmanCreateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbonmanUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(AbonmanUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _abonmanUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }
    }
}
