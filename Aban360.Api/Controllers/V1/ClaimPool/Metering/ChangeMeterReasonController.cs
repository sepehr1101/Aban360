using Aban360.ClaimPool.Application.Features.Metering.Services;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    [Route("v1/change-meter-reason")]
    public class ChangeMeterReasonController : BaseController
    {
        private readonly IChangeMeterReasonService _service;

        public ChangeMeterReasonController(IChangeMeterReasonService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ChangeMeterReasonGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ChangeMeterReasonGetDto> changeMeterReasons = await _service.GetAllAsync(cancellationToken);
            return Ok(changeMeterReasons);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ChangeMeterReasonGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(ChangeMeterReasonEnum id, CancellationToken cancellationToken)
        {
            ChangeMeterReasonGetDto changeMeterReasons = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(changeMeterReasons);
        }
    }
}
