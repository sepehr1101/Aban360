using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/ServiceLink-modified-bills-detail")]
    public class ServiceLinkModifiedBillsDetailController : BaseController
    {
        private readonly IServiceLinkModifiedBillsDetailHandler _modifiedBillsHandler;
        private readonly IReportGenerator _reportGenerator;
        public ServiceLinkModifiedBillsDetailController(
            IServiceLinkModifiedBillsDetailHandler modifiedBillsHandler,
            IReportGenerator reportGenerator)
        {
            _modifiedBillsHandler = modifiedBillsHandler;
            _modifiedBillsHandler.NotNull(nameof(modifiedBillsHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkModifiedBillsHeaderOutputDto, ServiceLinkModifiedBillsDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ServiceLinkModifiedBillsInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkModifiedBillsHeaderOutputDto, ServiceLinkModifiedBillsDetailDataOutputDto> modifiedBills = await _modifiedBillsHandler.Handle(input, cancellationToken);
            return Ok(modifiedBills);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ServiceLinkModifiedBillsInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _modifiedBillsHandler.Handle, CurrentUser, ReportLiterals.ServiceLinkModifiedBillsDetail, connectionId);
            return Ok(inputDto);
        }
    }
}
