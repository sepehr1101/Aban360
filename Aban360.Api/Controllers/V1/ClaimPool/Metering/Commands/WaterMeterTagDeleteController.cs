using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("water-meter-tag")]
    public class WaterMeterTagDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterTagDeleteHandler _waterMeterTagHandler;
        public WaterMeterTagDeleteController(
            IUnitOfWork uow,
            IWaterMeterTagDeleteHandler waterMeterTagHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterTagHandler = waterMeterTagHandler;
            _waterMeterTagHandler.NotNull(nameof(waterMeterTagHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] WaterMeterTagDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _waterMeterTagHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
