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

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition
{
    [Route("v1/sewage-equipment-broker")]
    public class SewageEquipmentBrokerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISewageEquipmentBrokerCreateHandler _sewageEquipmentBrokerCreateHandler;
        private readonly ISewageEquipmentBrokerUpdateHandler _sewageEquipmentBrokerUpdateHandler;
        private readonly ISewageEquipmentBrokerDeleteHandler _sewageEquipmentBrokerDeleteHandler;
        private readonly ISewageEquipmentBrokerGetSingleHandler _sewageEquipmentBrokerGetSingleHandler;
        private readonly ISewageEquipmentBrokerGetAllHandler _sewageEquipmentBrokerGetAllHandler;

        public SewageEquipmentBrokerController(
            IUnitOfWork uow,
            ISewageEquipmentBrokerCreateHandler sewageEquipmentBrokerCreateHandler,
            ISewageEquipmentBrokerUpdateHandler sewageEquipmentBrokerUpdateHandler,
            ISewageEquipmentBrokerDeleteHandler sewageEquipmentBrokerDeleteHandler,
            ISewageEquipmentBrokerGetSingleHandler sewageEquipmentBrokerGetSingleHandler,
            ISewageEquipmentBrokerGetAllHandler sewageEquipmentBrokerGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _sewageEquipmentBrokerCreateHandler = sewageEquipmentBrokerCreateHandler;
            _sewageEquipmentBrokerCreateHandler.NotNull(nameof(sewageEquipmentBrokerCreateHandler));

            _sewageEquipmentBrokerUpdateHandler = sewageEquipmentBrokerUpdateHandler;
            _sewageEquipmentBrokerUpdateHandler.NotNull(nameof(sewageEquipmentBrokerUpdateHandler));

            _sewageEquipmentBrokerDeleteHandler = sewageEquipmentBrokerDeleteHandler;
            _sewageEquipmentBrokerDeleteHandler.NotNull(nameof(sewageEquipmentBrokerDeleteHandler));

            _sewageEquipmentBrokerGetSingleHandler = sewageEquipmentBrokerGetSingleHandler;
            _sewageEquipmentBrokerGetSingleHandler.NotNull(nameof(sewageEquipmentBrokerGetSingleHandler));

            _sewageEquipmentBrokerGetAllHandler = sewageEquipmentBrokerGetAllHandler;
            _sewageEquipmentBrokerGetAllHandler.NotNull(nameof(sewageEquipmentBrokerGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SewageEquipmentBrokerCreateDto createDto, CancellationToken cancellationToken)
        {
            await _sewageEquipmentBrokerCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
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

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] SewageEquipmentBrokerDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _sewageEquipmentBrokerDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var sewageEquipmentBrokers = await _sewageEquipmentBrokerGetSingleHandler.Handle(id, cancellationToken);
            return Ok(sewageEquipmentBrokers);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SewageEquipmentBrokerGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var sewageEquipmentBrokers = await _sewageEquipmentBrokerGetAllHandler.Handle(cancellationToken);
            return Ok(sewageEquipmentBrokers);
        }
    }
}
