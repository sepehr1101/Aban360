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
    [Route("v1/offering-group")]
    public class OfferingGroupController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IOfferingGroupCreateHandler _offeringGroupCreateHandler;
        private readonly IOfferingGroupUpdateHandler _offeringGroupUpdateHandler;
        private readonly IOfferingGroupDeleteHandler _offeringGroupDeleteHandler;
        private readonly IOfferingGroupGetSingleHandler _offeringGroupGetSingleHandler;
        private readonly IOfferingGroupGetAllHandler _offeringGroupGetAllHandler;

        public OfferingGroupController(
            IUnitOfWork uow,
            IOfferingGroupCreateHandler offeringGroupCreateHandler,
            IOfferingGroupUpdateHandler offeringGroupUpdateHandler,
            IOfferingGroupDeleteHandler offeringGroupDeleteHandler,
            IOfferingGroupGetSingleHandler offeringGroupGetSingleHandler,
            IOfferingGroupGetAllHandler offeringGroupGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _offeringGroupCreateHandler = offeringGroupCreateHandler;
            _offeringGroupCreateHandler.NotNull(nameof(offeringGroupCreateHandler));

            _offeringGroupUpdateHandler = offeringGroupUpdateHandler;
            _offeringGroupUpdateHandler.NotNull(nameof(offeringGroupUpdateHandler));

            _offeringGroupDeleteHandler = offeringGroupDeleteHandler;
            _offeringGroupDeleteHandler.NotNull(nameof(offeringGroupDeleteHandler));

            _offeringGroupGetSingleHandler = offeringGroupGetSingleHandler;
            _offeringGroupGetSingleHandler.NotNull(nameof(offeringGroupGetSingleHandler));

            _offeringGroupGetAllHandler = offeringGroupGetAllHandler;
            _offeringGroupGetAllHandler.NotNull(nameof(offeringGroupGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingGroupCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] OfferingGroupCreateDto createDto, CancellationToken cancellationToken)
        {
            await _offeringGroupCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingGroupUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] OfferingGroupUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _offeringGroupUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingGroupDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] OfferingGroupDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _offeringGroupDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OfferingGroupGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            OfferingGroupGetDto offeringGroups = await _offeringGroupGetSingleHandler.Handle(id, cancellationToken);
            return Ok(offeringGroups);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<OfferingGroupGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<OfferingGroupGetDto> offeringGroups = await _offeringGroupGetAllHandler.Handle(cancellationToken);
            return Ok(offeringGroups);
        }
    }
}
