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
    [Route("v1/empty-unit-by-interval")]
    public class EmptyUnitByIntervalController : BaseController
    {
        private readonly IEmptyUnitByIntervalHandler _emptyUnitByInterval;
        private readonly IReportGenerator _reportGenerator;
        public EmptyUnitByIntervalController(
            IEmptyUnitByIntervalHandler emptyUnitByInterval,
            IReportGenerator reportGenerator)
        {
            _emptyUnitByInterval = emptyUnitByInterval;
            _emptyUnitByInterval.NotNull(nameof(_emptyUnitByInterval));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(EmptyUnitByIntervalInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<EmptyUnitHeaderOutputDto, EmptyUnitDataOutputDto> emptyUnit = await _emptyUnitByInterval.Handle(inputDto, cancellationToken);
            return Ok(emptyUnit);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, EmptyUnitByIntervalInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _emptyUnitByInterval.Handle, CurrentUser, ReportLiterals.EmptyUnit, connectionId);
            return Ok(inputDto);
        }
    }
}
