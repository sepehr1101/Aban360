using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Commands.Update.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Commands
{
    [Route("v1/table1")]
    public class Table1UpdateController : BaseController
    {
        private readonly ITable1UpdateHandler _table1UpdateHandler;
        public Table1UpdateController(ITable1UpdateHandler table1UpdateHandler)
        {
            _table1UpdateHandler = table1UpdateHandler;
            _table1UpdateHandler.NotNull(nameof(table1UpdateHandler));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<Table1UpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Table1UpdateDto UpdateDto, CancellationToken cancellationToken)
        {
            await _table1UpdateHandler.Handle(UpdateDto, cancellationToken);
            return Ok(UpdateDto);
        }
    }
}
