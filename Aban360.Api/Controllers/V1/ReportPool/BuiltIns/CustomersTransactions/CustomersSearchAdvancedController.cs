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
    [Route("v1/customers-search-advanced")]
    public class CustomersSearchAdvancedController : BaseController
    {
        private readonly ICustomerSearchAdvancedHandler _customerSearchAdvancedHandler; 
        private readonly IReportGenerator _reportGenerator;
        public CustomersSearchAdvancedController(
            ICustomerSearchAdvancedHandler customerSearchAdvancedHandler,
            IReportGenerator reportGenerator)
        {
            _customerSearchAdvancedHandler = customerSearchAdvancedHandler;
            _customerSearchAdvancedHandler.NotNull(nameof(customerSearchAdvancedHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(CustomerSearchAdvancedInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto> customer = await _customerSearchAdvancedHandler.Handle(input, cancellationToken);
            return Ok(customer);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, CustomerSearchAdvancedInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _customerSearchAdvancedHandler.Handle, CurrentUser, ReportLiterals.CustomerSearch, connectionId);
            return Ok(inputDto);
        }
    }
}
