// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.InstallationPool.Definition;
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

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.InstallationPool.Definition
{
    [Route("v1/equipment-broker")]
    public class EquipmentBrokerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEquipmentBrokerCreateHandler _equipmentBrokerCreateHandler;
        private readonly IEquipmentBrokerUpdateHandler _equipmentBrokerUpdateHandler;
        private readonly IEquipmentBrokerDeleteHandler _equipmentBrokerDeleteHandler;
        private readonly IEquipmentBrokerGetSingleHandler _equipmentBrokerGetSingleHandler;
        private readonly IEquipmentBrokerGetAllHandler _equipmentBrokerGetAllHandler;

        public EquipmentBrokerController(
            IUnitOfWork uow,
            IEquipmentBrokerCreateHandler equipmentBrokerCreateHandler,
            IEquipmentBrokerUpdateHandler equipmentBrokerUpdateHandler,
            IEquipmentBrokerDeleteHandler equipmentBrokerDeleteHandler,
            IEquipmentBrokerGetSingleHandler equipmentBrokerGetSingleHandler,
            IEquipmentBrokerGetAllHandler equipmentBrokerGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _equipmentBrokerCreateHandler = equipmentBrokerCreateHandler;
            _equipmentBrokerCreateHandler.NotNull(nameof(equipmentBrokerCreateHandler));

            _equipmentBrokerUpdateHandler = equipmentBrokerUpdateHandler;
            _equipmentBrokerUpdateHandler.NotNull(nameof(equipmentBrokerUpdateHandler));

            _equipmentBrokerDeleteHandler = equipmentBrokerDeleteHandler;
            _equipmentBrokerDeleteHandler.NotNull(nameof(equipmentBrokerDeleteHandler));

            _equipmentBrokerGetSingleHandler = equipmentBrokerGetSingleHandler;
            _equipmentBrokerGetSingleHandler.NotNull(nameof(equipmentBrokerGetSingleHandler));

            _equipmentBrokerGetAllHandler = equipmentBrokerGetAllHandler;
            _equipmentBrokerGetAllHandler.NotNull(nameof(equipmentBrokerGetAllHandler));
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

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _equipmentBrokerUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
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

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var equipmentBrokers = await _equipmentBrokerGetSingleHandler.Handle(id, cancellationToken);
            return Ok(equipmentBrokers);
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
