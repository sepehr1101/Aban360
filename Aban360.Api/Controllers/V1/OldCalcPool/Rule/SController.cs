using Aban360.Api.Filters;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule
{
    [Route("v1/s")]
    public class SController : BaseController
    {
        private readonly ISCreateHandler _sCreateHandler;
        private readonly ISDeleteHandler _sDeleteHandler;
        private readonly ISUpdateHandler _sUpdateHandler;
        private readonly ISGetAllHandler _sGetAllHandler;
        private readonly ISGetByIdHandler _sGetByIdHandler;
        private readonly ISGetHandler _sGetHandler;

        public SController(
            ISCreateHandler sCreateHandler,
            ISDeleteHandler sDeleteHandler,
            ISUpdateHandler sUpdateHandler,
            ISGetAllHandler sGetAllHandler,
            ISGetByIdHandler sGetByIdHandler,
            ISGetHandler sGetHandler)
        {
            _sCreateHandler = sCreateHandler;
            _sCreateHandler.NotNull(nameof(sCreateHandler));
            _sDeleteHandler = sDeleteHandler;
            _sDeleteHandler.NotNull(nameof(sDeleteHandler));
            _sUpdateHandler = sUpdateHandler;
            _sUpdateHandler.NotNull(nameof(sUpdateHandler));
            _sGetAllHandler = sGetAllHandler;
            _sGetAllHandler.NotNull(nameof(sGetAllHandler));
            _sGetByIdHandler = sGetByIdHandler;
            _sGetByIdHandler.NotNull(nameof(sGetByIdHandler));
            _sGetHandler = sGetHandler;
            _sGetHandler.NotNull(nameof(sGetHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SCreateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("SCreate")]
        public async Task<IActionResult> Create(SCreateDto CreateDto, CancellationToken cancellationToken)
        {
            await _sCreateHandler.Handle(CreateDto, CurrentUser, cancellationToken);
            return Ok(CreateDto);
        }

        [HttpPost]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        [EndpointAuthorization("SDelete")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _sDeleteHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SUpdateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("SUpdate")]
        public async Task<IActionResult> Update(SUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _sUpdateHandler.Handle(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<SGetDto> result = await _sGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            SGetDto result = await _sGetByIdHandler.Handle(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken)
        {
            IEnumerable<SGetDto> result = await _sGetHandler.Handle(fromDateJalali, toDateJalali, cancellationToken);
            return Ok(result);
        }
    }
}
