using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("water-meter-tag")]
    public class WaterMeterTagGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterTagGetSingleHandler _waterMeterTagHandler;
        public WaterMeterTagGetSingleController(
            IUnitOfWork uow,
            IWaterMeterTagGetSingleHandler waterMeterTagHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterTagHandler = waterMeterTagHandler;
            _waterMeterTagHandler.NotNull(nameof(waterMeterTagHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(string billId, CancellationToken cancellationToken)
        {
            var WaterMeterTag = await _waterMeterTagHandler.Handle(billId, cancellationToken);
            return Ok(WaterMeterTag);
        }
    }
}
