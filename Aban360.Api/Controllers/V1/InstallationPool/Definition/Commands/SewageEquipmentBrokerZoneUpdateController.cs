using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/sewage-equipment-broker-zone")]
    public class SewageEquipmentBrokerZoneUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISewageEquipmentBrokerZoneUpdateHandler _sewageEquipmentBrokerZoneUpdateHandler;
        public SewageEquipmentBrokerZoneUpdateController(
            IUnitOfWork uow,
            ISewageEquipmentBrokerZoneUpdateHandler sewageEquipmentBrokerZoneUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _sewageEquipmentBrokerZoneUpdateHandler = sewageEquipmentBrokerZoneUpdateHandler;
            _sewageEquipmentBrokerZoneUpdateHandler.NotNull(nameof(sewageEquipmentBrokerZoneUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerZoneUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SewageEquipmentBrokerZoneUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _sewageEquipmentBrokerZoneUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
