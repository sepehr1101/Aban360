using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Create.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/table1")]
    public class Table1CreateController : BaseController
    {
        private readonly ITable1CreateHandler _table1CreateHandler;
        public Table1CreateController(ITable1CreateHandler table1CreateHandler)
        {
            _table1CreateHandler = table1CreateHandler;
            _table1CreateHandler.NotNull(nameof(table1CreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Table1CreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(Table1CreateDto createDto, CancellationToken cancellationToken)
        {
            await _table1CreateHandler.Handle(createDto, cancellationToken);
            return Ok(createDto);
        }
    }
}
