using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Create.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Delete.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill
{
    [Route("v1/offering-unit")]
    public class OfferingUnitController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingUnitCreateHandler _offeringUnitCreateHandler;
        private readonly IOfferingUnitUpdateHandler _offeringUnitUpdateHandler;
        private readonly IOfferingUnitDeleteHandler _offeringUnitDeleteHandler;
        private readonly IOfferingUnitGetSingleHandler _offeringUnitGetSingleHandler;
        private readonly IOfferingUnitGetAllHandler _offeringUnitGetAllHandler;

        public OfferingUnitController(
            IUnitOfWork uow,
            IOfferingUnitCreateHandler offeringUnitCreateHandler,
            IOfferingUnitUpdateHandler offeringUnitUpdateHandler,
            IOfferingUnitDeleteHandler offeringUnitDeleteHandler,
            IOfferingUnitGetSingleHandler offeringUnitGetSingleHandler,
            IOfferingUnitGetAllHandler offeringUnitGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringUnitCreateHandler = offeringUnitCreateHandler;
            _offeringUnitCreateHandler.NotNull(nameof(offeringUnitCreateHandler));

            _offeringUnitUpdateHandler = offeringUnitUpdateHandler;
            _offeringUnitUpdateHandler.NotNull(nameof(offeringUnitUpdateHandler));

            _offeringUnitDeleteHandler = offeringUnitDeleteHandler;
            _offeringUnitDeleteHandler.NotNull(nameof(offeringUnitDeleteHandler));

            _offeringUnitGetSingleHandler = offeringUnitGetSingleHandler;
            _offeringUnitGetSingleHandler.NotNull(nameof(offeringUnitGetSingleHandler));

            _offeringUnitGetAllHandler = offeringUnitGetAllHandler;
            _offeringUnitGetAllHandler.NotNull(nameof(offeringUnitGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] OfferingUnitCreateDto createDto, CancellationToken cancellationToken)
        {
            await _offeringUnitCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingUnitUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] OfferingUnitUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _offeringUnitUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingUnitDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] OfferingUnitDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _offeringUnitDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingUnitGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            OfferingUnitGetDto offeringUnits = await _offeringUnitGetSingleHandler.Handle(id, cancellationToken);
            return Ok(offeringUnits);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<OfferingUnitGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<OfferingUnitGetDto> offeringUnits = await _offeringUnitGetAllHandler.Handle(cancellationToken);
            return Ok(offeringUnits);
        }
    }
}
