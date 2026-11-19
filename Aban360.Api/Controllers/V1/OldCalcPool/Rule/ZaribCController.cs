using Aban360.Api.Filters;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Application.Features.Rules.Services;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule
{
    [Route("v1/zarib-c")]
    public class ZaribCController : BaseController
    {
        private readonly IZaribCService _zaribCService;
        public ZaribCController(IZaribCService zaribCService)
        {
            _zaribCService = zaribCService;
            _zaribCService.NotNull(nameof(zaribCService));
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCCreateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("ZaribCCreate")]
        public async Task<IActionResult> Create(ZaribCCreateDto CreateDto, CancellationToken cancellationToken)
        {
            await _zaribCService.CreateAsync(CreateDto, CurrentUser, cancellationToken);
            return Ok(CreateDto);
        }

        [HttpPost]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        [EndpointAuthorization("ZaribCDelete")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _zaribCService.DeleteAsync(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCUpdateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("ZaribCUpdate")]
        public async Task<IActionResult> Update(ZaribCUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _zaribCService.UpdateAsync(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ZaribCQueryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ZaribCQueryDto> result = await _zaribCService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCQueryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken)
        {
            ZaribCQueryDto result = await _zaribCService.GetAsync(fromDateJalali, toDateJalali, cancellationToken);
            return Ok(result);
        }
    }
}
