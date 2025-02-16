using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("water-meter-tag-definition")]
    public class WaterMeterTagDefinitionGetAllController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterTagDefinitionGetAllHandler _tagDefinitionHandler;
        public WaterMeterTagDefinitionGetAllController(
            IUnitOfWork uow,
            IWaterMeterTagDefinitionGetAllHandler tagDefinitionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tagDefinitionHandler = tagDefinitionHandler;
            _tagDefinitionHandler.NotNull(nameof(tagDefinitionHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var waterMeterTagDefinition = await _tagDefinitionHandler.Handle(cancellationToken);
            return Ok(waterMeterTagDefinition);
        }
    }
}
