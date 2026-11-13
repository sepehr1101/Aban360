using Aban360.InstallationPool.Application.Features.Definition.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;




using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition
{
    [Route("v1/equipment-broker")]
    public class EquipmentBrokerController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEquipmentBrokerService _service;

        public EquipmentBrokerController(IUnitOfWork uow, IEquipmentBrokerService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EquipmentBrokerCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EquipmentBrokerUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EquipmentBrokerDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var equipmentBrokers = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(equipmentBrokers);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EquipmentBrokerGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var equipmentBrokers = await _service.GetAllAsync(cancellationToken);
            return Ok(equipmentBrokers);
        }
    }
}
