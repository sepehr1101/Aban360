using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Rules.Handlers.Queries.Contracts;
using Aban360.OldCalcPool.Domain.Features.Rules.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.OldCalcPool.Rule.Queries
{
    [Route("v1/s")]
    public class SGetByIdController : BaseController
    {
        private readonly ISGetByIdHandler _sGetByIdHandler;
        public SGetByIdController(ISGetByIdHandler sGetByIdHandler)
        {
            _sGetByIdHandler = sGetByIdHandler;
            _sGetByIdHandler.NotNull(nameof(sGetByIdHandler));
        }

        [HttpGet, HttpPost]
        [Route("get/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            SGetDto result = await _sGetByIdHandler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
