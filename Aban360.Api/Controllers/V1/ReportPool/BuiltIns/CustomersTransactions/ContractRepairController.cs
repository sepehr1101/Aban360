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
    [Route("v1/contract-repair")]
    public class ContractRepairController : BaseController
    {
        private readonly IContractRepairHandler _contractRepairHandler;
        private readonly IReportGenerator _reportGenerator;
        public ContractRepairController(
            IContractRepairHandler contractRepairHandler,
            IReportGenerator reportGenerator)
        {
            _contractRepairHandler = contractRepairHandler;
            _contractRepairHandler.NotNull(nameof(contractRepairHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ContractRepairHeaderOutputDto, ContractRepairDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ContractRepairInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<ContractRepairHeaderOutputDto, ContractRepairDataOutputDto> result = await _contractRepairHandler.Handle(input, cancellationToken);
            return Ok(result);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ContractRepairInputDto inputDto, CancellationToken cancellationToken)
        {
            string reportName = (inputDto.IsWater ? ReportLiterals.WaterRequestSummary : ReportLiterals.SewageRequestSummary) + ReportLiterals.ByUsageAndZone;
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _contractRepairHandler.Handle, CurrentUser, reportName, connectionId);
            return Ok(inputDto);
        }
    }
}
