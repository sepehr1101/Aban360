// Frozen consumer before service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers15.Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    [Route("v1/change-meter-reason")]
    public class ChangeMeterReasonController : BaseController
    {
        private readonly IChangeMeterReasonGetAllHandler _changeMeterReasonGetAllHandler;
        private readonly IChangeMeterReasonGetSingleHandler _changeMeterReasonGetSingleHandler;

        public ChangeMeterReasonController(
            IChangeMeterReasonGetAllHandler changeMeterReasonGetAllHandler,
            IChangeMeterReasonGetSingleHandler changeMeterReasonGetSingleHandler)
        {
            _changeMeterReasonGetAllHandler = changeMeterReasonGetAllHandler;
            _changeMeterReasonGetAllHandler.NotNull(nameof(changeMeterReasonGetAllHandler));
            _changeMeterReasonGetSingleHandler = changeMeterReasonGetSingleHandler;
            _changeMeterReasonGetSingleHandler.NotNull(nameof(changeMeterReasonGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<ChangeMeterReasonGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<ChangeMeterReasonGetDto> changeMeterReasons = await _changeMeterReasonGetAllHandler.Handle(cancellationToken);
            return Ok(changeMeterReasons);
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
