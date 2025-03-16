using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
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
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterTagGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            ICollection<WaterMeterTagGetDto> WaterMeterTag = await _waterMeterTagHandler.Handle(id, cancellationToken);
            return Ok(WaterMeterTag);
        }
    }
}
