using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/water-meter-tag")]
    public class WaterMeterTagUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterTagUpdateHandler _waterMeterTagHandler;
        public WaterMeterTagUpdateController(
            IUnitOfWork uow,
            IWaterMeterTagUpdateHandler waterMeterTagHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterTagHandler = waterMeterTagHandler;
            _waterMeterTagHandler.NotNull(nameof(waterMeterTagHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterTagUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _waterMeterTagHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
