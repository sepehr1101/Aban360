using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/sewage-equipment-broker")]
    public class SewageEquipmentBrokerUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISewageEquipmentBrokerUpdateHandler _sewageEquipmentBrokerUpdateHandler;
        public SewageEquipmentBrokerUpdateController(
            IUnitOfWork uow,
            ISewageEquipmentBrokerUpdateHandler sewageEquipmentBrokerUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _sewageEquipmentBrokerUpdateHandler = sewageEquipmentBrokerUpdateHandler;
            _sewageEquipmentBrokerUpdateHandler.NotNull(nameof(sewageEquipmentBrokerUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] SewageEquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _sewageEquipmentBrokerUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
