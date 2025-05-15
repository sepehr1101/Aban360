using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/sewage-equipment-broker")]
    public class SewageEquipmentBrokerCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISewageEquipmentBrokerCreateHandler _sewageEquipmentBrokerCreateHandler;
        public SewageEquipmentBrokerCreateController(
            IUnitOfWork uow,
            ISewageEquipmentBrokerCreateHandler sewageEquipmentBrokerCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _sewageEquipmentBrokerCreateHandler = sewageEquipmentBrokerCreateHandler;
            _sewageEquipmentBrokerCreateHandler.NotNull(nameof(sewageEquipmentBrokerCreateHandler));
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
    }
}
