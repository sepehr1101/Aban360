using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Commands;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Input;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/removed-bill")]
    public class BillsCanRemovedController : BaseController
    {
        private readonly IBillsCanRemovedGetHandler _billsCanRemovedHandler;
        public BillsCanRemovedController(IBillsCanRemovedGetHandler billsCanRemovedHandler)
        {
            _billsCanRemovedHandler = billsCanRemovedHandler;
            _billsCanRemovedHandler.NotNull(nameof(billsCanRemovedHandler));
        }

        [HttpPost, HttpGet]
        [Route("can-removed/get")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<BillsCanRemovedOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCanRemoved(SearchInput inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<BillsCanRemovedOutputDto> result = await _billsCanRemovedHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
