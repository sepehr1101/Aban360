using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.ServiceLink
{
    [Route("v1/service-link-other-expenses-items")]
    public class ServiceLinkOtherExpensesItemsController : BaseController
    {
        private readonly IOtherExpensesItemsInsertHandler _otherExpensesItemsInsertHandler;
        private readonly IOtherExpensesItemsRemoveHandler _otherExpensesItemsRemoveHandler;
        private readonly IOtherExpensesItemsGetAllHandler _otherExpensesItemsGetAllHandler;
        private readonly IOtherExpensesItemsGetByIdHandler _otherExpensesItemsGetByIdaHandler;
        private readonly IOtherExpensesItemsGetByItemIdHandler _otherExpensesItemsGetByItemIdHandler;
        public ServiceLinkOtherExpensesItemsController(
            IOtherExpensesItemsInsertHandler otherExpensesItemsInsertHandler,
            IOtherExpensesItemsRemoveHandler otherExpensesItemsRemoveHandler,
            IOtherExpensesItemsGetAllHandler otherExpensesItemsGetAllHandler,
            IOtherExpensesItemsGetByIdHandler otherExpensesItemsGetByIdaHandler,
            IOtherExpensesItemsGetByItemIdHandler otherExpensesItemsGetByItemIdHandler)
        {
            _otherExpensesItemsInsertHandler = otherExpensesItemsInsertHandler;
            _otherExpensesItemsInsertHandler.NotNull(nameof(otherExpensesItemsInsertHandler));

            _otherExpensesItemsRemoveHandler = otherExpensesItemsRemoveHandler;
            _otherExpensesItemsRemoveHandler.NotNull(nameof(otherExpensesItemsRemoveHandler));

            _otherExpensesItemsGetAllHandler = otherExpensesItemsGetAllHandler;
            _otherExpensesItemsGetAllHandler.NotNull(nameof(otherExpensesItemsGetAllHandler));

            _otherExpensesItemsGetByIdaHandler = otherExpensesItemsGetByIdaHandler;
            _otherExpensesItemsGetByIdaHandler.NotNull(nameof(otherExpensesItemsGetByIdaHandler));

            _otherExpensesItemsGetByItemIdHandler = otherExpensesItemsGetByItemIdHandler;
            _otherExpensesItemsGetByItemIdHandler.NotNull(nameof(otherExpensesItemsGetByItemIdHandler));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OtherExpensesItemsInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] OtherExpensesItemsInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _otherExpensesItemsInsertHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("remove/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove(int id, CancellationToken cancellationToken)
        {
            await _otherExpensesItemsRemoveHandler.Handle(id, CurrentUser, cancellationToken);
            return Ok(id);
        }

        [HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OtherExpensesItemsGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            OtherExpensesItemsGetDto data = await _otherExpensesItemsGetByIdaHandler.Handle(id, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Route("get-itemId/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OtherExpensesItemsGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByItemId(int id, CancellationToken cancellationToken)
        {
            OtherExpensesItemsGetDto data = await _otherExpensesItemsGetByItemIdHandler.Handle(id, cancellationToken);
            return Ok(data);
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<OtherExpensesItemsGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<OtherExpensesItemsGetDto> data = await _otherExpensesItemsGetAllHandler.Handle(cancellationToken);
            return Ok(data);
        }
    }
}
