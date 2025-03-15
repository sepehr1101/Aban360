using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("water-meter-tag")]
    public class WaterMeterTagGetSingleBySearchInputController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterTagGetSinglBySearchInputeHandler _waterMeterTagGetSinglBySearchInputeHandler;
        public WaterMeterTagGetSingleBySearchInputController(
            IUnitOfWork uow,
            IWaterMeterTagGetSinglBySearchInputeHandler waterMeterTagGetSinglBySearchInputeHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterTagGetSinglBySearchInputeHandler = waterMeterTagGetSinglBySearchInputeHandler;
            _waterMeterTagGetSinglBySearchInputeHandler.NotNull(nameof(waterMeterTagGetSinglBySearchInputeHandler));
        }

        [HttpPost, HttpGet]
        [Route("search/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(string input, CancellationToken cancellationToken)
        {
            WaterMeterTagGetDto WaterMeterTag = await _waterMeterTagGetSinglBySearchInputeHandler.Handle(input, cancellationToken);
            return Ok(WaterMeterTag);
        }
    }
}
