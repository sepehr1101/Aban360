using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/meter-diameter")]
    public class MeterDiameterGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMeterDiameterGetSingleHandler _meterDiameterHandler;
        public MeterDiameterGetSingleController(
            IUnitOfWork uow,
            IMeterDiameterGetSingleHandler meterDiameterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _meterDiameterHandler = meterDiameterHandler;
            _meterDiameterHandler.NotNull(nameof(meterDiameterHandler));
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<MeterDiameterGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(short id, CancellationToken cancellationToken)
        {
            var meterDiameter = await _meterDiameterHandler.Handle(id, cancellationToken);
            return Ok(meterDiameter);
        }
    }
}
