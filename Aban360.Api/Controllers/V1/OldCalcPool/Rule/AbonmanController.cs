using Aban360.Api.Filters;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule
{
    [Route("v1/abonman")]
    public class AbonmanController : BaseController
    {
        private readonly IAbonmanCreateHandler _abonmanCreateHandler;
        private readonly IAbonmanUpdateHandler _abonmanUpdateHandler;
        private readonly IAbonmanGetAllHandler _abonmanGetAllHandler;
        private readonly IAbonmanGetHandler _abonmanGetHandler;

        public AbonmanController(
            IAbonmanCreateHandler abonmanCreateHandler,
            IAbonmanUpdateHandler abonmanUpdateHandler,
            IAbonmanGetAllHandler abonmanGetAllHandler,
            IAbonmanGetHandler abonmanGetHandler)
        {
            _abonmanCreateHandler = abonmanCreateHandler;
            _abonmanCreateHandler.NotNull(nameof(abonmanCreateHandler));
            _abonmanUpdateHandler = abonmanUpdateHandler;
            _abonmanUpdateHandler.NotNull(nameof(abonmanUpdateHandler));
            _abonmanGetAllHandler = abonmanGetAllHandler;
            _abonmanGetAllHandler.NotNull(nameof(abonmanGetAllHandler));
            _abonmanGetHandler = abonmanGetHandler;
            _abonmanGetHandler.NotNull(nameof(abonmanGetHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbonmanCreateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("AbonmanCommand")]
        public async Task<IActionResult> Create(AbonmanCreateDto inputDto, CancellationToken cancellationToken)
        {
            await _abonmanCreateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update")]
        [EndpointAuthorization("AbonmanCommand")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbonmanUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(AbonmanUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _abonmanUpdateHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet, HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<AbonmanGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<AbonmanGetDto> result = await _abonmanGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbonmanGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            AbonmanGetDto result = await _abonmanGetHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
