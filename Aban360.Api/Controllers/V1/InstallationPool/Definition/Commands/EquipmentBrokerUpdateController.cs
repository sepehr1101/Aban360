using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/equipment-broker")]
    public class EquipmentBrokerUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEquipmentBrokerUpdateHandler _equipmentBrokerUpdateHandler;
        public EquipmentBrokerUpdateController(
            IUnitOfWork uow,
            IEquipmentBrokerUpdateHandler equipmentBrokerUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _equipmentBrokerUpdateHandler = equipmentBrokerUpdateHandler;
            _equipmentBrokerUpdateHandler.NotNull(nameof(equipmentBrokerUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _equipmentBrokerUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }

}
