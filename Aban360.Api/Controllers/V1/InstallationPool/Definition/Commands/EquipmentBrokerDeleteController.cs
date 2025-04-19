using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Contracts;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/equipment-broker")]
    public class EquipmentBrokerDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEquipmentBrokerDeleteHandler _equipmentBrokerDeleteHandler;
        public EquipmentBrokerDeleteController(
            IUnitOfWork uow,
            IEquipmentBrokerDeleteHandler equipmentBrokerDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _equipmentBrokerDeleteHandler = equipmentBrokerDeleteHandler;
            _equipmentBrokerDeleteHandler.NotNull(nameof(equipmentBrokerDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EquipmentBrokerDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _equipmentBrokerDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }

}
