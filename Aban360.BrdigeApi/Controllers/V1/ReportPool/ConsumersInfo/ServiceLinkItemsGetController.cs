using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.OldCalcPool.Application.Features.Db70.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/service-link")]
    public class ServiceLinkItemsGetController : BaseController
    {
        private readonly IOfferingGetAllHandler _offeringGetHandler;
        public ServiceLinkItemsGetController(IOfferingGetAllHandler offeringGetHandler)
        {
            _offeringGetHandler = offeringGetHandler;
            _offeringGetHandler.NotNull(nameof(offeringGetHandler));
        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetItems(CancellationToken cancellationToken)
        {
            IEnumerable<NumericDictionary> result = await _offeringGetHandler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
