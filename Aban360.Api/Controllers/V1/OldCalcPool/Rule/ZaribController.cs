using Aban360.Api.Filters;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Aban360.OldCalcPool.Application.Features.Rules.Services;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule
{
    [Route("v1/zarib")]
    public class ZaribController : BaseController
    {
        private readonly IZaribService _zaribService;
        public ZaribController(IZaribService zaribService)
        {
            _zaribService = zaribService;
            _zaribService.NotNull(nameof(zaribService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribCreateInputDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("ZaribCreate")]
        public async Task<IActionResult> Create(ZaribCreateInputDto createDto, CancellationToken cancellationToken)
        {
            await _zaribService.CreateAsync(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribUpdateInputDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("ZaribUpdate")]
        public async Task<IActionResult> Update(ZaribUpdateInputDto UpdateDto, CancellationToken cancellationToken)
        {
            await _zaribService.UpdateAsync(UpdateDto, CurrentUser, cancellationToken);
            return Ok(UpdateDto);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ZaribGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ZaribGetDto> result = await _zaribService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ZaribGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            ZaribGetDto result = await _zaribService.GetAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
