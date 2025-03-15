using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/change-meter-reason")]
    public class ChangeMeterReasonGetAllController : BaseController
    {
        private readonly IChangeMeterReasonGetAllHandler _changeMeterReasonGetAllHandler;
        public ChangeMeterReasonGetAllController(IChangeMeterReasonGetAllHandler changeMeterReasonGetAllHandler)
        {
            _changeMeterReasonGetAllHandler = changeMeterReasonGetAllHandler;
            _changeMeterReasonGetAllHandler.NotNull(nameof(changeMeterReasonGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ChangeMeterReasonGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ChangeMeterReasonGetDto> changeMeterReasons = await _changeMeterReasonGetAllHandler.Handle(cancellationToken);
            return Ok(changeMeterReasons);
        }
    }
}
