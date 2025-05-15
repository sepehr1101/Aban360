using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Queries
{
    [Route("v1/sewage-equipment-broker")]
    public class SewageEquipmentBrokerGetAllController : BaseController
    {
        private readonly ISewageEquipmentBrokerGetAllHandler _sewageEquipmentBrokerGetAllHandler;
        public SewageEquipmentBrokerGetAllController(ISewageEquipmentBrokerGetAllHandler sewageEquipmentBrokerGetAllHandler)
        {
            _sewageEquipmentBrokerGetAllHandler = sewageEquipmentBrokerGetAllHandler;
            _sewageEquipmentBrokerGetAllHandler.NotNull(nameof(sewageEquipmentBrokerGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SewageEquipmentBrokerGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var sewageEquipmentBrokers = await _sewageEquipmentBrokerGetAllHandler.Handle(cancellationToken);
            return Ok(sewageEquipmentBrokers);
        }
    }
}
