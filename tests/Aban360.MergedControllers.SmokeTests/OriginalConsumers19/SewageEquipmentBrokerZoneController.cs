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
    [Route("v1/sewage-equipment-broker-zone")]
    public class SewageEquipmentBrokerZoneController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISewageEquipmentBrokerZoneCreateHandler _sewageEquipmentBrokerZoneCreateHandler;
        private readonly ISewageEquipmentBrokerZoneUpdateHandler _sewageEquipmentBrokerZoneUpdateHandler;
        private readonly ISewageEquipmentBrokerZoneDeleteHandler _sewageEquipmentBrokerZoneDeleteHandler;
        private readonly ISewageEquipmentBrokerZoneGetSingleHandler _sewageEquipmentBrokerZoneGetSingleHandler;
        private readonly ISewageEquipmentBrokerZoneGetAllHandler _sewageEquipmentBrokerZoneGetAllHandler;

        public SewageEquipmentBrokerZoneController(
            IUnitOfWork uow,
            ISewageEquipmentBrokerZoneCreateHandler sewageEquipmentBrokerZoneCreateHandler,
            ISewageEquipmentBrokerZoneUpdateHandler sewageEquipmentBrokerZoneUpdateHandler,
            ISewageEquipmentBrokerZoneDeleteHandler sewageEquipmentBrokerZoneDeleteHandler,
            ISewageEquipmentBrokerZoneGetSingleHandler sewageEquipmentBrokerZoneGetSingleHandler,
            ISewageEquipmentBrokerZoneGetAllHandler sewageEquipmentBrokerZoneGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _sewageEquipmentBrokerZoneCreateHandler = sewageEquipmentBrokerZoneCreateHandler;
            _sewageEquipmentBrokerZoneCreateHandler.NotNull(nameof(sewageEquipmentBrokerZoneCreateHandler));

            _sewageEquipmentBrokerZoneUpdateHandler = sewageEquipmentBrokerZoneUpdateHandler;
            _sewageEquipmentBrokerZoneUpdateHandler.NotNull(nameof(sewageEquipmentBrokerZoneUpdateHandler));

            _sewageEquipmentBrokerZoneDeleteHandler = sewageEquipmentBrokerZoneDeleteHandler;
            _sewageEquipmentBrokerZoneDeleteHandler.NotNull(nameof(sewageEquipmentBrokerZoneDeleteHandler));

            _sewageEquipmentBrokerZoneGetSingleHandler = sewageEquipmentBrokerZoneGetSingleHandler;
            _sewageEquipmentBrokerZoneGetSingleHandler.NotNull(nameof(sewageEquipmentBrokerZoneGetSingleHandler));

            _sewageEquipmentBrokerZoneGetAllHandler = sewageEquipmentBrokerZoneGetAllHandler;
            _sewageEquipmentBrokerZoneGetAllHandler.NotNull(nameof(sewageEquipmentBrokerZoneGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerZoneCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SewageEquipmentBrokerZoneCreateDto createDto, CancellationToken cancellationToken)
        {
            await _sewageEquipmentBrokerZoneCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
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

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerZoneDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SewageEquipmentBrokerZoneDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _sewageEquipmentBrokerZoneDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerZoneGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var sewageEquipmentBrokerZones = await _sewageEquipmentBrokerZoneGetSingleHandler.Handle(id, cancellationToken);
            return Ok(sewageEquipmentBrokerZones);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SewageEquipmentBrokerZoneGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var sewageEquipmentBrokerZones = await _sewageEquipmentBrokerZoneGetAllHandler.Handle(cancellationToken);
            return Ok(sewageEquipmentBrokerZones);
        }
    }
}
