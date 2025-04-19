using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Queries
{
    [Route("v1/equipment_broker")]
    public class EquipmentBrokerGetAllController : BaseController
    {
        private readonly IEquipmentBrokerGetAllHandler _equipmentBrokerGetAllHandler;
        public EquipmentBrokerGetAllController(IEquipmentBrokerGetAllHandler equipmentBrokerGetAllHandler)
        {
            _equipmentBrokerGetAllHandler = equipmentBrokerGetAllHandler;
            _equipmentBrokerGetAllHandler.NotNull(nameof(equipmentBrokerGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EquipmentBrokerGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var equipmentBrokers = await _equipmentBrokerGetAllHandler.Handle(cancellationToken);
            return Ok(equipmentBrokers);
        }
    }

}
