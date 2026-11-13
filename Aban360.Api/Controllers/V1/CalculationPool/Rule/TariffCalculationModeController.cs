using Aban360.CalculationPool.Application.Features.Rule.Services;
using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule
{
    [Route("v1/tariff-calculation-mode")]
    public class TariffCalculationModeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffCalculationModeService _service;

        public TariffCalculationModeController(IUnitOfWork uow, ITariffCalculationModeService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TariffCalculationModeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TariffCalculationModeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] TariffCalculationModeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCalculationModeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(TariffCalculationModeEnum id, CancellationToken cancellationToken)
        {
            TariffCalculationModeGetDto tariffCalculationModes = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(tariffCalculationModes);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffCalculationModeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<TariffCalculationModeGetDto> tariffCalculationModes = await _service.GetAllAsync(cancellationToken);
            return Ok(tariffCalculationModes);
        }
    }
}
