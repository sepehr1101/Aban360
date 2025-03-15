using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/change-meter-reason")]
    public class ChangeMeterReasonGetSingleController : BaseController
    {
        private readonly IChangeMeterReasonGetSingleHandler _changeMeterReasonGetSingleHandler;
        public ChangeMeterReasonGetSingleController(IChangeMeterReasonGetSingleHandler changeMeterReasonGetSingleHandler)
        {
            _changeMeterReasonGetSingleHandler = changeMeterReasonGetSingleHandler;
            _changeMeterReasonGetSingleHandler.NotNull(nameof(changeMeterReasonGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ChangeMeterReasonGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(ChangeMeterReasonEnum id, CancellationToken cancellationToken)
        {
            ChangeMeterReasonGetDto changeMeterReasons = await _changeMeterReasonGetSingleHandler.Handle(id, cancellationToken);
            return Ok(changeMeterReasons);
        }
    }

}
