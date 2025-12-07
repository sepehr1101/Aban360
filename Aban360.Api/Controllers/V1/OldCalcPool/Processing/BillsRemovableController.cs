using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Processing.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Processing.Dto.Queries.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Processing
{
    [Route("v1/bill")]
    public class BillsRemovableController : BaseController
    {
        private readonly IBillsCanRemovedGetHandler _billsCanRemovedHandler;
        public BillsRemovableController(IBillsCanRemovedGetHandler billsCanRemovedHandler)
        {
            _billsCanRemovedHandler = billsCanRemovedHandler;
            _billsCanRemovedHandler.NotNull(nameof(billsCanRemovedHandler));
        }

        [HttpPost]
        [Route("removable")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<BillsCanRemovedOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCanRemoved([FromBody]SearchInput inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<BillsCanRemovedOutputDto> result = await _billsCanRemovedHandler.Handle(inputDto, cancellationToken);
            return Ok(result);
        }
    }
}
