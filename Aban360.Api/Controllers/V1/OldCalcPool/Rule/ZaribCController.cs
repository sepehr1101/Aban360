using Aban360.Api.Filters;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Delete.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule
{
    public class ZaribCController : BaseController
    {
        private readonly IZaribCCreateHandler _c0_zaribCCreateHandler;
        private readonly IZaribCDeleteHandler _c1_zaribCDeleteHandler;
        private readonly IZaribCUpdateHandler _c2_zaribCUpdateHandler;
        private readonly IZaribCGetAllHandler _c3_zaribCGetAllHandler;
        private readonly IZaribCGetHandler _c4_zaribCGetHandler;
        public ZaribCController(IZaribCCreateHandler c0_zaribCCreateHandler, IZaribCDeleteHandler c1_zaribCDeleteHandler, IZaribCUpdateHandler c2_zaribCUpdateHandler, IZaribCGetAllHandler c3_zaribCGetAllHandler, IZaribCGetHandler c4_zaribCGetHandler)
        {
            _c0_zaribCCreateHandler = c0_zaribCCreateHandler;
            _c0_zaribCCreateHandler.NotNull(nameof(c0_zaribCCreateHandler));
            _c1_zaribCDeleteHandler = c1_zaribCDeleteHandler;
            _c1_zaribCDeleteHandler.NotNull(nameof(c1_zaribCDeleteHandler));
            _c2_zaribCUpdateHandler = c2_zaribCUpdateHandler;
            _c2_zaribCUpdateHandler.NotNull(nameof(c2_zaribCUpdateHandler));
            _c3_zaribCGetAllHandler = c3_zaribCGetAllHandler;
            _c3_zaribCGetAllHandler.NotNull(nameof(c3_zaribCGetAllHandler));
            _c4_zaribCGetHandler = c4_zaribCGetHandler;
            _c4_zaribCGetHandler.NotNull(nameof(c4_zaribCGetHandler));
        }

        [HttpPost]
        [Route("~/v1/zarib-c/Create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCCreateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("ZaribCCreate")]
        public async Task<IActionResult> Create(ZaribCCreateDto CreateDto, CancellationToken cancellationToken)
        {
            await _c0_zaribCCreateHandler.Handle(CreateDto, CurrentUser, cancellationToken);
            return Ok(CreateDto);
        }

        [HttpPost]
        [Route("~/v1/zarib-c/delete/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        [EndpointAuthorization("ZaribCDelete")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _c1_zaribCDeleteHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost]
        [Route("~/v1/zarib-c/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCUpdateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("ZaribCUpdate")]
        public async Task<IActionResult> Update(ZaribCUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _c2_zaribCUpdateHandler.Handle(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/zarib-c/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ZaribCQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ZaribCQueryDto> result = await _c3_zaribCGetAllHandler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("~/v1/zarib-c/get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCQueryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken)
        {
            ZaribCQueryDto result = await _c4_zaribCGetHandler.Handle(fromDateJalali, toDateJalali, cancellationToken);
            return Ok(result);
        }
    }
}
