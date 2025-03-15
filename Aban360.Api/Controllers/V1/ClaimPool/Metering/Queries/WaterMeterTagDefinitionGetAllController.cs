using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/water-meter-tag-definition")]
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
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterTagDefinitionGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<WaterMeterTagDefinitionGetDto> waterMeterTagDefinition = await _tagDefinitionHandler.Handle(cancellationToken);
            return Ok(waterMeterTagDefinition);
        }
    }
}
