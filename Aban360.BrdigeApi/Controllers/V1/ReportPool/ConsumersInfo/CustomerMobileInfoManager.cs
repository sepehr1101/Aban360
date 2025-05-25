using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/customer")]
    public class CustomerMobileInfoManager : BaseController
    {
        private readonly ICustomerMobileInfoListHandler _customerMobileInfoListHandler;
        public CustomerMobileInfoManager(ICustomerMobileInfoListHandler customerMobileInfoListHandler)
        {
            _customerMobileInfoListHandler = customerMobileInfoListHandler;
            _customerMobileInfoListHandler.NotNull(nameof(_customerMobileInfoListHandler));
        }

        [HttpPost]
        [Route("mobile-numbers")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<BillIdMobileDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMobiles([FromBody] BillIdListDtoWrapper billIdListDtoWrapper, CancellationToken cancellationToken)
        {
            IEnumerable<BillIdMobileDto> billIdMobileDtos = await _customerMobileInfoListHandler.Handle(billIdListDtoWrapper, cancellationToken);
            return Ok(billIdMobileDtos);
        }
    }  
}
