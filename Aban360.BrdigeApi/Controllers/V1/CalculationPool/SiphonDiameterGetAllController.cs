using Aban360.CalculationPool.Application.Features.Sale.Handlers.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/siphon-diameter")]
    public class SiphonDiameterGetAllController : BaseController
    {
        private readonly ISiphonDiameterGetAllHandler _getHandler;
        public SiphonDiameterGetAllController(ISiphonDiameterGetAllHandler getHandler)
        {
            _getHandler = getHandler;
            _getHandler.NotNull(nameof(getHandler));
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _getHandler.Handle(cancellationToken);

            return Ok(result);
        }
    }
}
