using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/table1")]
    public class Table1GetAllController : BaseController
    {
        private readonly ITable1GetAllHandler _table1GetAllHandler;
        public Table1GetAllController(ITable1GetAllHandler table1GetAllHandler)
        {
            _table1GetAllHandler = table1GetAllHandler;
            _table1GetAllHandler.NotNull(nameof(table1GetAllHandler));
        }

        [HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<Table1GetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
        {
            IEnumerable<Table1GetDto> result = await _table1GetAllHandler.Handle( cancellationToken);
            return Ok(result);
        }
    }
}
