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
    [Route("v1/nerkh")]
    public class NerkhController : BaseController
    {
        private readonly INerkhCreateHandler _nerkhCreateHandler;
        private readonly INerkhUpdateHandler _nerkhUpdateHandler;
        private readonly INerkhGetAllHandler _nerkhGetAllHandler;
        private readonly INerkhGetHandler _nerkhGetHandler;

        public NerkhController(
            INerkhCreateHandler nerkhCreateHandler,
            INerkhUpdateHandler nerkhUpdateHandler,
            INerkhGetAllHandler nerkhGetAllHandler,
            INerkhGetHandler nerkhGetHandler)
        {
            _nerkhCreateHandler = nerkhCreateHandler;
            _nerkhCreateHandler.NotNull(nameof(nerkhCreateHandler));
            _nerkhUpdateHandler = nerkhUpdateHandler;
            _nerkhUpdateHandler.NotNull(nameof(nerkhUpdateHandler));
            _nerkhGetAllHandler = nerkhGetAllHandler;
            _nerkhGetAllHandler.NotNull(nameof(nerkhGetAllHandler));
            _nerkhGetHandler = nerkhGetHandler;
            _nerkhGetHandler.NotNull(nameof(nerkhGetHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NerkhCreateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("NerkhCreate")]
        public async Task<IActionResult> Create(NerkhCreateDto createDto, CancellationToken cancellationToken)
        {
            await _nerkhCreateHandler.Handle(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NerkhUpdateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("NerkhUpdate")]
        public async Task<IActionResult> Update(NerkhUpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            await _nerkhUpdateHandler.Handle(UpdateDto, CurrentUser, cancellationToken);
            return Ok(UpdateDto);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NerkhGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<NerkhGetDto> result = await _nerkhGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NerkhGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            NerkhGetDto result = await _nerkhGetHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
