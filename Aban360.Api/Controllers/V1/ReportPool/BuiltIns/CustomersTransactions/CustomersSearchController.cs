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
    [Route("v1/customers-search")]
    public class CustomersSearchController : BaseController
    {
        private readonly ICustomerSearchHandler _customerSearchHandler;
        private readonly IReportGenerator _reportGenerator;
        public CustomersSearchController(
            ICustomerSearchHandler customerSearchHandler,
            IReportGenerator reportGenerator)
        {
            _customerSearchHandler = customerSearchHandler;
            _customerSearchHandler.NotNull(nameof(customerSearchHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(CustomerSearchInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto> customer = await _customerSearchHandler.Handle(input, cancellationToken);
            return Ok(customer);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, CustomerSearchInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _customerSearchHandler.Handle, CurrentUser, ReportLiterals.CustomerSearch, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(CustomerSearchInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 40;
            ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto> customer = await _customerSearchHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(customer, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
