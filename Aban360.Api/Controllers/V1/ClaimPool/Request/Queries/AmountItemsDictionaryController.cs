using Aban360.ClaimPool.Application.Features.Request.Handler.Queries.Contracts;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Aban360.Api.Controllers.V1.ClaimPool.Request.Queries
{
    [Route("v1/amount-items")]
    public class AmountItemsDictionaryController : BaseController
    {
        private readonly IServiceGroupGetAllHandler _serviceGroupGetAllHandler;
        public AmountItemsDictionaryController(IServiceGroupGetAllHandler serviceGroupGetAllHandler)
        {
            _serviceGroupGetAllHandler = serviceGroupGetAllHandler;
            _serviceGroupGetAllHandler.NotNull(nameof(serviceGroupGetAllHandler));
        }

        [HttpGet]
        [Route("dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDictionary([Optional] bool isReturn, CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _serviceGroupGetAllHandler.Handle(isReturn, cancellationToken);
            return Ok(result);
        }
    }
}
