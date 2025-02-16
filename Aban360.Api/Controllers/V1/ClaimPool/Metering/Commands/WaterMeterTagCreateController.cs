using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("water-meter-tag")]
    public class WaterMeterTagCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterTagCreateHandler _waterMeterTagHandler;
        public WaterMeterTagCreateController(
            IUnitOfWork uow,
            IWaterMeterTagCreateHandler waterMeterTagHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterTagHandler = waterMeterTagHandler;
            _waterMeterTagHandler.NotNull(nameof(waterMeterTagHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] WaterMeterTagCreateDto createDto, CancellationToken cancellationToken)
        {
            await _waterMeterTagHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
