using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/meter")]
    public class ChangeMeterCauseGetAllController : BaseController
    {
        private readonly IChangeMeterCauseGetAllHandler _changeMeterCause;
        public ChangeMeterCauseGetAllController(
            IChangeMeterCauseGetAllHandler changeMeterCause)
        {
            _changeMeterCause = changeMeterCause;
            _changeMeterCause.NotNull(nameof(_changeMeterCause));
        }

        [HttpPost, HttpGet]
        [Route("change-cuase")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeCause(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _changeMeterCause.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
