using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/customer-mini-search")]
    public class CustomerMiniSearchController : BaseController
    {
        private readonly ICustomerMiniSearchHandler _customerMiniSearch;
        private readonly IReportGenerator _reportGenerator;
        public CustomerMiniSearchController(
            ICustomerMiniSearchHandler customerMiniSearch,
            IReportGenerator reportGenerator)
        {
            _customerMiniSearch = customerMiniSearch;
            _customerMiniSearch.NotNull(nameof(_customerMiniSearch));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CustomerMiniSearchHeaderOutputDto, CustomerMiniSearchDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(CustomerMiniSearchInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerMiniSearchHeaderOutputDto, CustomerMiniSearchDataOutputDto> CustomerMiniSearch = await _customerMiniSearch.Handle(inputDto, cancellationToken);
            return Ok(CustomerMiniSearch);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, CustomerMiniSearchInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _customerMiniSearch.Handle, CurrentUser, ReportLiterals.CustomerSearch, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(CustomerMiniSearchInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 42;
            ReportOutput<CustomerMiniSearchHeaderOutputDto, CustomerMiniSearchDataOutputDto> CustomerMiniSearch = await _customerMiniSearch.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(CustomerMiniSearch, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
