using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/modify-cause")]
    public class ModifyCauseController : BaseController
    {
        private readonly IModifyCauseGetAllHandler _modifyCauseHandler;
        public ModifyCauseController(IModifyCauseGetAllHandler modifyCauseHandler)
        {
            _modifyCauseHandler = modifyCauseHandler;
            _modifyCauseHandler.NotNull(nameof(modifyCauseHandler));
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> modifyCauses = await _modifyCauseHandler.Handle(cancellationToken);
            return Ok(modifyCauses);
        }
    }
}
