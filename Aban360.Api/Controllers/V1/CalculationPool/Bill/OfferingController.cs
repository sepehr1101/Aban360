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
    [Route("v1/offering")]
    public class OfferingController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingCreateHandler _offeringCreateHandler;
        private readonly IOfferingUpdateHandler _offeringUpdateHandler;
        private readonly IOfferingDeleteHandler _offeringDeleteHandler;
        private readonly IOfferingGetSingleHandler _offeringGetSingleHandler;
        private readonly IOfferingGetAllHandler _offeringGetAllHandler;

        public OfferingController(
            IUnitOfWork uow,
            IOfferingCreateHandler offeringCreateHandler,
            IOfferingUpdateHandler offeringUpdateHandler,
            IOfferingDeleteHandler offeringDeleteHandler,
            IOfferingGetSingleHandler offeringGetSingleHandler,
            IOfferingGetAllHandler offeringGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringCreateHandler = offeringCreateHandler;
            _offeringCreateHandler.NotNull(nameof(offeringCreateHandler));

            _offeringUpdateHandler = offeringUpdateHandler;
            _offeringUpdateHandler.NotNull(nameof(offeringUpdateHandler));

            _offeringDeleteHandler = offeringDeleteHandler;
            _offeringDeleteHandler.NotNull(nameof(offeringDeleteHandler));

            _offeringGetSingleHandler = offeringGetSingleHandler;
            _offeringGetSingleHandler.NotNull(nameof(offeringGetSingleHandler));

            _offeringGetAllHandler = offeringGetAllHandler;
            _offeringGetAllHandler.NotNull(nameof(offeringGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] OfferingCreateDto createDto, CancellationToken cancellationToken)
        {
            await _offeringCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] OfferingUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _offeringUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] OfferingDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _offeringDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            OfferingGetDto offerings = await _offeringGetSingleHandler.Handle(id, cancellationToken);
            return Ok(offerings);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<OfferingGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<OfferingGetDto> offerings = await _offeringGetAllHandler.Handle(cancellationToken);
            return Ok(offerings);
        }
    }
}
