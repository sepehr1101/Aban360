using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/water-meter")]
    public class WaterMeterGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterGetSingleHandler _waterMeterHandler;
        public WaterMeterGetSingleController(
            IUnitOfWork uow,
            IWaterMeterGetSingleHandler waterMeterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterHandler = waterMeterHandler;
            _waterMeterHandler.NotNull(nameof(waterMeterHandler));
        }

        [HttpGet,HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            WaterMeterGetDto waterMeter = await _waterMeterHandler.Handle(id,cancellationToken);
            return Ok(waterMeter);
        }
    }
}
