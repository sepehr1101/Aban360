using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using ITariffService = global::Aban360.CalculationPool.Application.Features.Rule.Services.ITariffService;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule
{
    [Route("v1/tariff")]
    public class TariffController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffService _tariffService;
        public TariffController(IUnitOfWork uow, ITariffService tariffService)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _tariffService = tariffService;
            _tariffService.NotNull(nameof(tariffService));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TariffCreateDto createDto, CancellationToken cancellationToken)
        {
            await _tariffService.TariffCreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TariffUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _tariffService.TariffUpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] TariffDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _tariffService.TariffDeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            TariffGetDto tariffs = await _tariffService.TariffGetSingleAsync(id, cancellationToken);
            return Ok(tariffs);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<TariffGetDto> tariffs = await _tariffService.TariffGetAllAsync(cancellationToken);
            return Ok(tariffs);
        }

        [HttpPost]
        [Route("duplicate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffDuplicateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Duplicate([FromBody] TariffDuplicateDto DuplicateDto, CancellationToken cancellationToken)
        {
            await _tariffService.TariffDuplicateAsync(DuplicateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(DuplicateDto);
        }

        [HttpPost, HttpGet]
        [ActionName("GetByOfferingId")]
        [Route("by-line-item-type/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByLineItemTypeId(short id, CancellationToken cancellationToken)
        {
            ICollection<TariffGetDto> tariffs = await _tariffService.TariffGetByLineItemTypeIdAsync(id, cancellationToken);
            return Ok(tariffs);
        }

        [HttpPost, HttpGet]
        [Route("by-offering/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByOfferingId(short id, CancellationToken cancellationToken)
        {
            ICollection<TariffGetDto> tariffs = await _tariffService.TariffGetByOfferingIdAsync(id, cancellationToken);
            return Ok(tariffs);
        }
    }
}
