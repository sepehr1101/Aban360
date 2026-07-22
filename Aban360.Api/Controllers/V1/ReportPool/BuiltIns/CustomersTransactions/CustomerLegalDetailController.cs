using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/customer")]
    public class CustomerLegalDetailController : BaseController
    {
        private readonly ICustomerLegalDetailHandler _detailHandler;
        private readonly IReportGenerator _reportGenerator;
        public CustomerLegalDetailController(
            ICustomerLegalDetailHandler detailHandler,
            IReportGenerator reportGenerator)
        {
            _detailHandler = detailHandler;
            _detailHandler.NotNull(nameof(detailHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("legal-detail-raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CustomerLegalDetailHeaderOutputDto, CustomerLegalDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DetailRaw([FromBody] CustomerLegalInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerLegalDetailHeaderOutputDto, CustomerLegalDetailDataOutputDto> result = await _detailHandler.Handle(input, cancellationToken);
            return Ok(result);
        }
        
        [HttpPost]
        [Route("legal-detail-excel/{connectionId}")]
        public async Task<IActionResult> DetailExcel(string connectionId,[FromBody] CustomerLegalInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerLegalDetailHeaderOutputDto, CustomerLegalDetailDataOutputDto> result = await _detailHandler.Handle(input, cancellationToken);
            await _reportGenerator.FireAndInform(input, cancellationToken, _detailHandler.Handle, CurrentUser, ReportLiterals.CustomerLegalDetail, connectionId);
            return Ok(input);
        }
    }
}
