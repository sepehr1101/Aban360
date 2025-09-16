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
    [Route("v1/household-number")]
    public class HouseholdNumberController : BaseController
    {
        private readonly IHouseholdNumberHandler _householdNumber;
        private readonly IReportGenerator _reportGenerator;
        public HouseholdNumberController(IHouseholdNumberHandler householdNumber,
            IReportGenerator reportGenerator)
        {
            _householdNumber = householdNumber;
            _householdNumber.NotNull(nameof(_householdNumber));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(HouseholdNumberInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberDataOutputDto> householdNumber = await _householdNumber.Handle(inputDto, cancellationToken);
            return Ok(householdNumber);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, HouseholdNumberInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _householdNumber.Handle, CurrentUser, ReportLiterals.HouseholdNumberDetail, connectionId);
            return Ok(inputDto);
        }
    }
}
