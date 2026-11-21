using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Application.Features.Rule.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.Rule.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Rule
{
    [Route("v1/tariff")]
    public class TariffController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITariffCreateHandler _tariffCreateHandler;
        private readonly ITariffUpdateHandler _tariffUpdateHandler;
        private readonly ITariffDeleteHandler _tariffDeleteHandler;
        private readonly ITariffGetSingleHandler _tariffGetSingleHandler;
        private readonly ITariffGetAllHandler _tariffGetAllHandler;
        private readonly ITariffDuplicateHandler _tariffDuplicateHandler;
        private readonly ITariffGetByLineItemTypeIdHandler _tariffGetByLineItemTypeIdHandler;
        private readonly ITariffGetByOfferingIdHandler _tariffGetByOfferingIdHandler;

        public TariffController(
            IUnitOfWork uow,
            ITariffCreateHandler tariffCreateHandler,
            ITariffUpdateHandler tariffUpdateHandler,
            ITariffDeleteHandler tariffDeleteHandler,
            ITariffGetSingleHandler tariffGetSingleHandler,
            ITariffGetAllHandler tariffGetAllHandler,
            ITariffDuplicateHandler tariffDuplicateHandler,
            ITariffGetByLineItemTypeIdHandler tariffGetByLineItemTypeIdHandler,
            ITariffGetByOfferingIdHandler tariffGetByOfferingIdHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _tariffCreateHandler = tariffCreateHandler;
            _tariffCreateHandler.NotNull(nameof(tariffCreateHandler));

            _tariffUpdateHandler = tariffUpdateHandler;
            _tariffUpdateHandler.NotNull(nameof(tariffUpdateHandler));

            _tariffDeleteHandler = tariffDeleteHandler;
            _tariffDeleteHandler.NotNull(nameof(tariffDeleteHandler));

            _tariffGetSingleHandler = tariffGetSingleHandler;
            _tariffGetSingleHandler.NotNull(nameof(tariffGetSingleHandler));

            _tariffGetAllHandler = tariffGetAllHandler;
            _tariffGetAllHandler.NotNull(nameof(tariffGetAllHandler));

            _tariffDuplicateHandler = tariffDuplicateHandler;
            _tariffDuplicateHandler.NotNull(nameof(tariffDuplicateHandler));

            _tariffGetByLineItemTypeIdHandler = tariffGetByLineItemTypeIdHandler;
            _tariffGetByLineItemTypeIdHandler.NotNull(nameof(tariffGetByLineItemTypeIdHandler));

            _tariffGetByOfferingIdHandler = tariffGetByOfferingIdHandler;
            _tariffGetByOfferingIdHandler.NotNull(nameof(tariffGetByOfferingIdHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TariffCreateDto createDto, CancellationToken cancellationToken)
        {
            await _tariffCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TariffUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _tariffUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] TariffDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _tariffDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            TariffGetDto tariffs = await _tariffGetSingleHandler.Handle(id, cancellationToken);
            return Ok(tariffs);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<TariffGetDto> tariffs = await _tariffGetAllHandler.Handle(cancellationToken);
            return Ok(tariffs);
        }

        [HttpPost]
        [Route("duplicate")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TariffDuplicateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Duplicate([FromBody] TariffDuplicateDto DuplicateDto, CancellationToken cancellationToken)
        {
            await _tariffDuplicateHandler.Handle(DuplicateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(DuplicateDto);
        }

        [HttpPost, HttpGet]
        [ActionName("GetByOfferingId")]
        [Route("by-line-item-type/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByLineItemTypeId(short id, CancellationToken cancellationToken)
        {
            ICollection<TariffGetDto> tariffs = await _tariffGetByLineItemTypeIdHandler.Handle(id, cancellationToken);
            return Ok(tariffs);
        }

        [HttpPost, HttpGet]
        [Route("by-offering/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TariffGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByOfferingId(short id, CancellationToken cancellationToken)
        {
            ICollection<TariffGetDto> tariffs = await _tariffGetByOfferingIdHandler.Handle(id, cancellationToken);
            return Ok(tariffs);
        }
    }
}
