using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Implementations;
using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.CalculationPool.Application.Features.ServiceLink.Services;
using Aban360.Api.Controllers.V1.CalculationPool.ServiceLink;

namespace Aban360.Api.Controllers.V1.CalculationPool.ServiceLink
{
    [Route("v1/service-link-other-expenses-items")]
    public class ServiceLinkOtherExpensesItemsController : BaseController
    {
        private readonly IOtherExpensesItemsService _otherExpensesItemsService;
        public ServiceLinkOtherExpensesItemsController(IOtherExpensesItemsService otherExpensesItemsService)
        {
            _otherExpensesItemsService = otherExpensesItemsService;
            _otherExpensesItemsService.NotNull(nameof(otherExpensesItemsService));
        }

        [HttpPost]
        [Route("insert")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OtherExpensesItemsInsertInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] OtherExpensesItemsInsertInputDto inputDto, CancellationToken cancellationToken)
        {
            await _otherExpensesItemsService.OtherExpensesItemsInsertAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("remove")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OtherExpensesItemsRemoveInputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Remove([FromBody] OtherExpensesItemsRemoveInputDto inputDto, CancellationToken cancellationToken)
        {
            await _otherExpensesItemsService.OtherExpensesItemsRemoveAsync(inputDto.Id, CurrentUser, cancellationToken);
            return Ok(inputDto);
        }

        [HttpGet]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OtherExpensesItemsDataDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            OtherExpensesItemsDataDto data = await _otherExpensesItemsService.OtherExpensesItemsGetByIdAsync(id, cancellationToken);
            return Ok(data);
        }

        [HttpGet]
        [Route("get/{billId}/{itemId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<OtherExpensesItemsDataDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByItemId(string billId, int itemId, CancellationToken cancellationToken)
        {
            OtherExpensesItemsDataDto data = await _otherExpensesItemsService.OtherExpensesItemsGetByItemIdAsync(billId, itemId, cancellationToken);
            return Ok(data);
        }

        [HttpPost, HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<OtherExpensesItemsDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<OtherExpensesItemsDataDto> data = await _otherExpensesItemsService.OtherExpensesItemsGetAllAsync(cancellationToken);
            return Ok(data);
        }
    }
}
