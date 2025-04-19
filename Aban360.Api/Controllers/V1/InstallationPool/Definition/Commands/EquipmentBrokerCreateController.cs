using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/equipment_broker")]
    public class EquipmentBrokerCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEquipmentBrokerCreateHandler _equipmentBrokerCreateHandler;
        public EquipmentBrokerCreateController(
            IUnitOfWork uow,
            IEquipmentBrokerCreateHandler equipmentBrokerCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _equipmentBrokerCreateHandler = equipmentBrokerCreateHandler;
            _equipmentBrokerCreateHandler.NotNull(nameof(equipmentBrokerCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EquipmentBrokerCreateDto createDto, CancellationToken cancellationToken)
        {
            await _equipmentBrokerCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
