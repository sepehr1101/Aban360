using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("water-meter-tag-definition")]
    public class WaterMeterTagDefinitionCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterTagDefinitionCreateHandler _tagDefinitionHandler;
        public WaterMeterTagDefinitionCreateController(
            IUnitOfWork uow,
            IWaterMeterTagDefinitionCreateHandler tagDefinitionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tagDefinitionHandler = tagDefinitionHandler;
            _tagDefinitionHandler.NotNull(nameof(tagDefinitionHandler));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] WaterMeterTagDefinitionCreateDto createDto, CancellationToken cancellationToken)
        {
            await _tagDefinitionHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
