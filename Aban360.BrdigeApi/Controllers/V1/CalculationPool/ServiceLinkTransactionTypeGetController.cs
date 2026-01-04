using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/service-link")]
    public class ServiceLinkTransactionTypeGetController : BaseController
    {
        public ServiceLinkTransactionTypeGetController()
        {
        }

        [HttpGet]
        [Route("transaction-type")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOffering(CancellationToken cancellationToken)
        {
            ICollection<NumericDictionary> result = new List<NumericDictionary>()
            {
                new NumericDictionary(1,"بدهکار"),
                new NumericDictionary(2,"بستانکار")
            };
            return Ok(result);
        }
    }
}
