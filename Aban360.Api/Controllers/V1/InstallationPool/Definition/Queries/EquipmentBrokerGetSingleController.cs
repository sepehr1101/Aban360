using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Queries
{
    [Route("v1/equipment-broker")]
    public class EquipmentBrokerGetSingleController : BaseController
    {
        private readonly IEquipmentBrokerGetSingleHandler _equipmentBrokerGetSingleHandler;
        public EquipmentBrokerGetSingleController(IEquipmentBrokerGetSingleHandler equipmentBrokerGetSingleHandler)
        {
            _equipmentBrokerGetSingleHandler = equipmentBrokerGetSingleHandler;
            _equipmentBrokerGetSingleHandler.NotNull(nameof(equipmentBrokerGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var equipmentBrokers = await _equipmentBrokerGetSingleHandler.Handle(id, cancellationToken);
            return Ok(equipmentBrokers);
        }
    }

}
