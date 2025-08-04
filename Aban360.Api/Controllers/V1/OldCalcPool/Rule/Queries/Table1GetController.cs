using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/table1")]
    public class Table1GetController : BaseController
    {
        private readonly ITable1GetHandler _table1GetHandler;
        public Table1GetController(ITable1GetHandler table1GetHandler)
        {
            _table1GetHandler = table1GetHandler;
            _table1GetHandler.NotNull(nameof(table1GetHandler));
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Table1GetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            Table1GetDto result = await _table1GetHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
