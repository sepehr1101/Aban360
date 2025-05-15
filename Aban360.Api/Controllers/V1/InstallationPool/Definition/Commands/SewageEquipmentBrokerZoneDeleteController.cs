using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/sewage-equipment-broker-zone")]
    public class SewageEquipmentBrokerZoneDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISewageEquipmentBrokerZoneDeleteHandler _sewageEquipmentBrokerZoneDeleteHandler;
        public SewageEquipmentBrokerZoneDeleteController(
            IUnitOfWork uow,
            ISewageEquipmentBrokerZoneDeleteHandler sewageEquipmentBrokerZoneDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _sewageEquipmentBrokerZoneDeleteHandler = sewageEquipmentBrokerZoneDeleteHandler;
            _sewageEquipmentBrokerZoneDeleteHandler.NotNull(nameof(sewageEquipmentBrokerZoneDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerZoneDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SewageEquipmentBrokerZoneDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _sewageEquipmentBrokerZoneDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
