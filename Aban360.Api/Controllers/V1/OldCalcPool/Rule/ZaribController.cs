using Aban360.Api.Filters;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule
{
    public class ZaribController : BaseController
    {
        private readonly IZaribCreateHandler _c0_zaribCreateHandler;
        private readonly IZaribUpdateHandler _c1_zaribUpdateHandler;
        private readonly IZaribGetAllHandler _c2_zaribGetAllHandler;
        private readonly IZaribGetHandler _c3_zaribGetHandler;
        public ZaribController(IZaribCreateHandler c0_zaribCreateHandler, IZaribUpdateHandler c1_zaribUpdateHandler, IZaribGetAllHandler c2_zaribGetAllHandler, IZaribGetHandler c3_zaribGetHandler)
        {
            _c0_zaribCreateHandler = c0_zaribCreateHandler;
            _c0_zaribCreateHandler.NotNull(nameof(c0_zaribCreateHandler));
            _c1_zaribUpdateHandler = c1_zaribUpdateHandler;
            _c1_zaribUpdateHandler.NotNull(nameof(c1_zaribUpdateHandler));
            _c2_zaribGetAllHandler = c2_zaribGetAllHandler;
            _c2_zaribGetAllHandler.NotNull(nameof(c2_zaribGetAllHandler));
            _c3_zaribGetHandler = c3_zaribGetHandler;
            _c3_zaribGetHandler.NotNull(nameof(c3_zaribGetHandler));
        }

        [HttpPost]
        [Route("~/v1/zarib/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCreateInputDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("ZaribCreate")]
        public async Task<IActionResult> Create(ZaribCreateInputDto createDto, CancellationToken cancellationToken)
        {
            await _c0_zaribCreateHandler.Handle(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("~/v1/zarib/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribUpdateInputDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("ZaribUpdate")]
        public async Task<IActionResult> Update(ZaribUpdateInputDto UpdateDto, CancellationToken cancellationToken)
        {
            await _c1_zaribUpdateHandler.Handle(UpdateDto, CurrentUser, cancellationToken);
            return Ok(UpdateDto);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/zarib/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ZaribGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ZaribGetDto> result = await _c2_zaribGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/zarib/get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            ZaribGetDto result = await _c3_zaribGetHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
