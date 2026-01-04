using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/service-link")]
    public class ServiceLinkModifyAmountController : BaseController
    {
        public ServiceLinkModifyAmountController()
        {
        }

        [HttpPost]
        [Route("modify-amount")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<NumericDictionary>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ModifyAmount(ServiceLinkModifyAmountInputDto inputDto,CancellationToken cancellationToken)
        {
            return Ok(inputDto);
        }
    }
}
