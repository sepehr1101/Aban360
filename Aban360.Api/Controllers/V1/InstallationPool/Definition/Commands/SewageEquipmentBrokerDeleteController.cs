using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Contracts;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/sewage-equipment-broker")]
    public class SewageEquipmentBrokerDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISewageEquipmentBrokerDeleteHandler _sewageEquipmentBrokerDeleteHandler;
        public SewageEquipmentBrokerDeleteController(
            IUnitOfWork uow,
            ISewageEquipmentBrokerDeleteHandler sewageEquipmentBrokerDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _sewageEquipmentBrokerDeleteHandler = sewageEquipmentBrokerDeleteHandler;
            _sewageEquipmentBrokerDeleteHandler.NotNull(nameof(sewageEquipmentBrokerDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SewageEquipmentBrokerDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _sewageEquipmentBrokerDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
