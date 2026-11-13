using Aban360.Api.Controllers.V1;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Contracts;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers19.Aban360.Api.Controllers.V1.InstallationPool.Definition
{
    [Route("v1/equipment-broker-zone")]
    public class EquipmentBrokerZoneController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEquipmentBrokerZoneCreateHandler _equipmentBrokerZoneCreateHandler;
        private readonly IEquipmentBrokerZoneUpdateHandler _equipmentBrokerZoneUpdateHandler;
        private readonly IEquipmentBrokerZoneDeleteHandler _equipmentBrokerZoneDeleteHandler;
        private readonly IEquipmentBrokerZoneGetSingleHandler _equipmentBrokerZoneGetSingleHandler;
        private readonly IEquipmentBrokerZoneGetAllHandler _equipmentBrokerZoneGetAllHandler;

        public EquipmentBrokerZoneController(
            IUnitOfWork uow,
            IEquipmentBrokerZoneCreateHandler equipmentBrokerZoneCreateHandler,
            IEquipmentBrokerZoneUpdateHandler equipmentBrokerZoneUpdateHandler,
            IEquipmentBrokerZoneDeleteHandler equipmentBrokerZoneDeleteHandler,
            IEquipmentBrokerZoneGetSingleHandler equipmentBrokerZoneGetSingleHandler,
            IEquipmentBrokerZoneGetAllHandler equipmentBrokerZoneGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _equipmentBrokerZoneCreateHandler = equipmentBrokerZoneCreateHandler;
            _equipmentBrokerZoneCreateHandler.NotNull(nameof(equipmentBrokerZoneCreateHandler));

            _equipmentBrokerZoneUpdateHandler = equipmentBrokerZoneUpdateHandler;
            _equipmentBrokerZoneUpdateHandler.NotNull(nameof(equipmentBrokerZoneUpdateHandler));

            _equipmentBrokerZoneDeleteHandler = equipmentBrokerZoneDeleteHandler;
            _equipmentBrokerZoneDeleteHandler.NotNull(nameof(equipmentBrokerZoneDeleteHandler));

            _equipmentBrokerZoneGetSingleHandler = equipmentBrokerZoneGetSingleHandler;
            _equipmentBrokerZoneGetSingleHandler.NotNull(nameof(equipmentBrokerZoneGetSingleHandler));

            _equipmentBrokerZoneGetAllHandler = equipmentBrokerZoneGetAllHandler;
            _equipmentBrokerZoneGetAllHandler.NotNull(nameof(equipmentBrokerZoneGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerZoneCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EquipmentBrokerZoneCreateDto createDto, CancellationToken cancellationToken)
        {
            await _equipmentBrokerZoneCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerZoneUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EquipmentBrokerZoneUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _equipmentBrokerZoneUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerZoneDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EquipmentBrokerZoneDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _equipmentBrokerZoneDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerZoneGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var equipmentBrokerZones = await _equipmentBrokerZoneGetSingleHandler.Handle(id, cancellationToken);
            return Ok(equipmentBrokerZones);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EquipmentBrokerZoneGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var equipmentBrokerZones = await _equipmentBrokerZoneGetAllHandler.Handle(cancellationToken);
            return Ok(equipmentBrokerZones);
        }
    }
}
