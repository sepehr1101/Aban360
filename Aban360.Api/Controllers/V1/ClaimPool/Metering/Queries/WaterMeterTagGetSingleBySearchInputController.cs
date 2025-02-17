using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
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
        public async Task<IActionResult> Search(string input, CancellationToken cancellationToken)
        {
            var WaterMeterTag = await _waterMeterTagGetSinglBySearchInputeHandler.Handle(input, cancellationToken);
            return Ok(WaterMeterTag);
        }
    }
}
