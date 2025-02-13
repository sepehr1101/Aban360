using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("water-meter")]
    public class WaterMeterGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterGetAllHandler _waterMeterHandler;
        public WaterMeterGetAllController(
            IUnitOfWork uow,
            IWaterMeterGetAllHandler waterMeterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterHandler = waterMeterHandler;
            _waterMeterHandler.NotNull(nameof(waterMeterHandler));
        }

        [HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var waterMeter = await _waterMeterHandler.Handle(cancellationToken);
            return Ok(waterMeter);
        }
    }
}
