using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.ConsumersInfo.Queries.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/customer-general")]
    public class CustomerGeneralInfoController : BaseController
    {
        private readonly ICustomerGeneralInfoGetHandler _customerInfoHandle;
        private readonly IReportGenerator _reportGenerator;
        public CustomerGeneralInfoController(
            ICustomerGeneralInfoGetHandler customerInfoHandle,
            IReportGenerator reportGenerator)
        {
            _customerInfoHandle = customerInfoHandle;
            _customerInfoHandle.NotNull(nameof(customerInfoHandle));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));
        }

        [HttpPost]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> info(SearchInput searchInput, CancellationToken cancellationToken)
        {
            ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> summary = await _customerInfoHandle.Handle(searchInput, cancellationToken);
            return Ok(summary);
        }


        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, SearchInput searchInput, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(searchInput, cancellationToken, _customerInfoHandle.Handle, CurrentUser, ReportLiterals.CustomerGeneralInfo, connectionId);
            return Ok(searchInput);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(SearchInput searchInput, CancellationToken cancellationToken)
        {
            int reportCode = 800;
            ReportOutput<CustomerGeneralInfoHeaderDto, CustomerGeneralInfoDataDto> customerInfo = await _customerInfoHandle.Handle(searchInput, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(customerInfo, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
