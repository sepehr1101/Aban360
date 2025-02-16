using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("water-meter-tag-definition")]
    public class WaterMeterTagDefinitionGetSingleController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterTagDefinitionGetSingleHandler _tagDefinitionHandler;
        public WaterMeterTagDefinitionGetSingleController(
            IUnitOfWork uow,
            IWaterMeterTagDefinitionGetSingleHandler tagDefinitionHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tagDefinitionHandler = tagDefinitionHandler;
            _tagDefinitionHandler.NotNull(nameof(tagDefinitionHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var waterMeterTagDefinition = await _tagDefinitionHandler.Handle(id, cancellationToken);
            return Ok(waterMeterTagDefinition);
        }
    }
}
