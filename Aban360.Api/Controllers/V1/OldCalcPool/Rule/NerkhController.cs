using Aban360.Api.Filters;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Application.Features.Rules.Services;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule
{
    [Route("v1/nerkh")]
    public class NerkhController : BaseController
    {
        private readonly INerkhService _nerkhService;
        public NerkhController(INerkhService nerkhService)
        {
            _nerkhService = nerkhService;
            _nerkhService.NotNull(nameof(nerkhService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NerkhCreateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("NerkhCreate")]
        public async Task<IActionResult> Create(NerkhCreateDto createDto, CancellationToken cancellationToken)
        {
            await _nerkhService.CreateAsync(createDto, CurrentUser, cancellationToken);
            return Ok(createDto);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NerkhUpdateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("NerkhUpdate")]
        public async Task<IActionResult> Update(NerkhUpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            await _nerkhService.UpdateAsync(UpdateDto, CurrentUser, cancellationToken);
            return Ok(UpdateDto);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NerkhGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<NerkhGetDto> result = await _nerkhService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<NerkhGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            NerkhGetDto result = await _nerkhService.GetAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
