using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.CustomersTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.CustomersTransactions
{
    [Route("v1/non-premanent-branch")]
    public class NonPremanentBranchController : BaseController
    {
        private readonly INonPremanentBranchHandler _nonPremanentBranch;
        public NonPremanentBranchController(INonPremanentBranchHandler nonPremanentBranch)
        {
            _nonPremanentBranch = nonPremanentBranch;
            _nonPremanentBranch.NotNull(nameof(_nonPremanentBranch));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<NonPremanentBranchHeaderOutputDto, NonPremanentBranchDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(NonPremanentBranchInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<NonPremanentBranchHeaderOutputDto, NonPremanentBranchDataOutputDto> nonPremanentBranch = await _nonPremanentBranch.Handle(inputDto, cancellationToken);
            return Ok(nonPremanentBranch);
        }
    }
}
