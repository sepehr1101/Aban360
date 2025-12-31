using Aban360.CalculationPool.Application.Features.ServiceLink.Handler.Commands.Contracts;
using Aban360.CalculationPool.Domain.Features.ServiceLink;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.BrdigeApi.Controllers.V1.CalculationPool
{
    [Route("v1/service-link")]
    public class ServiceLinkInquiryDetailController : BaseController
    {
        private readonly IServiceLinkInquiryDetailHandler _serviceLinkInquiryDetailHandler;
        public ServiceLinkInquiryDetailController(IServiceLinkInquiryDetailHandler serviceLinkInquiryDetailHandler)
        {
            _serviceLinkInquiryDetailHandler = serviceLinkInquiryDetailHandler;
            _serviceLinkInquiryDetailHandler.NotNull(nameof(serviceLinkInquiryDetailHandler));
        }

        [HttpPost, HttpGet]
        [Route("inquiry")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<ServiceLinkInquiryOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> InquaryDetail(ServiceLinkInquiryInputDto inputDto, CancellationToken cancellationToken)
        {
            IEnumerable<ServiceLinkInquiryOutputDto> detail = await _serviceLinkInquiryDetailHandler.Handle(inputDto, cancellationToken);
            return Ok(detail);
        }
    }
}
