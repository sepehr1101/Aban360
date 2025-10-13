using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.WaterTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.WaterMeterTransactions
{
    [Route("v1/contractual-and-olgoo-level")]
    public class ContractualAndOlgooLevelController : BaseController
    {
        private readonly IContractualAndOlgooLevelHandler _contractualAndOlgooLevel;
        private readonly IReportGenerator _reportGenerator;
        public ContractualAndOlgooLevelController(
            IContractualAndOlgooLevelHandler contractualAndOlgooLevel,
            IReportGenerator reportGenerator)
        {
            _contractualAndOlgooLevel = contractualAndOlgooLevel;
            _contractualAndOlgooLevel.NotNull(nameof(_contractualAndOlgooLevel));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ContractualAndOlgooLevelHeaderOutputDto, ContractualAndOlgooLevelDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ContractualAndOlgooLevelInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ContractualAndOlgooLevelHeaderOutputDto, ContractualAndOlgooLevelDataOutputDto> debtorByDay = await _contractualAndOlgooLevel.Handle(inputDto, cancellationToken);
            return Ok(debtorByDay);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, ContractualAndOlgooLevelInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _contractualAndOlgooLevel.Handle, CurrentUser, ReportLiterals.ContractualAndOlgooLevel, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStiReport(ContractualAndOlgooLevelInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 450;
            ReportOutput<ContractualAndOlgooLevelHeaderOutputDto, ContractualAndOlgooLevelDataOutputDto> result = await _contractualAndOlgooLevel.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
