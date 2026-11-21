using Aban360.Api.Filters;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.OldCalcPool.Application.Features.Rules.Services;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule
{
    [Route("v1/abonman")]
    public class AbonmanController : BaseController
    {
        private readonly IAbonmanService _abonmanService;
        public AbonmanController(IAbonmanService abonmanService)
        {
            _abonmanService = abonmanService;
            _abonmanService.NotNull(nameof(abonmanService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbonmanCreateDto>), StatusCodes.Status200OK)]
        [EndpointAuthorization("AbonmanCommand")]
        public async Task<IActionResult> Create(AbonmanCreateDto inputDto, CancellationToken cancellationToken)
        {
            await _abonmanService.CreateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("update")]
        [EndpointAuthorization("AbonmanCommand")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbonmanUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(AbonmanUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _abonmanService.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet, HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<AbonmanGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<AbonmanGetDto> result = await _abonmanService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<AbonmanGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            AbonmanGetDto result = await _abonmanService.GetAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
