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
    [Route("v1/contractual-capacity")]
    public class ContractualCapacityController : BaseController
    {
        private readonly IContractualCapacityHandler _contractualCapacity;
        private readonly IReportGenerator _reportGenerator;
        public ContractualCapacityController(
            IContractualCapacityHandler contractualCapacity,
            IReportGenerator reportGenerator)
        {
            _contractualCapacity = contractualCapacity;
            _contractualCapacity.NotNull(nameof(_contractualCapacity));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ContractualCapacityInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto> contractualCapacity = await _contractualCapacity.Handle(inputDto, cancellationToken);
            return Ok(contractualCapacity);
        }


        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ContractualCapacityInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _contractualCapacity.Handle, CurrentUser, ReportLiterals.ContractualCapacity, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ContractualCapacityInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 30;
            ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto> result = await _contractualCapacity.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
