using Aban360.Api.Filters;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Application.Features.Rules.Services;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule
{
    [Route("v1/s")]
    public class SController : BaseController
    {
        private readonly ISService _sService;
        public SController(ISService sService)
        {
            _sService = sService;
            _sService.NotNull(nameof(sService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SCreateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("SCreate")]
        public async Task<IActionResult> Create(SCreateDto CreateDto, CancellationToken cancellationToken)
        {
            await _sService.CreateAsync(CreateDto, CurrentUser, cancellationToken);
            return Ok(CreateDto);
        }

        [HttpPost]
        [Route("delete/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        [EndpointAuthorization("SDelete")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _sService.DeleteAsync(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SUpdateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("SUpdate")]
        public async Task<IActionResult> Update(SUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _sService.UpdateAsync(updateDto, CurrentUser, cancellationToken);
            return Ok(updateDto);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<SGetDto> result = await _sService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            SGetDto result = await _sService.GetByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<SGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string fromDateJalali, string toDateJalali, CancellationToken cancellationToken)
        {
            IEnumerable<SGetDto> result = await _sService.GetAsync(fromDateJalali, toDateJalali, cancellationToken);
            return Ok(result);
        }
    }
}
