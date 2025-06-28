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
    public class NonPermanentBranchController : BaseController
    {
        private readonly INonPermanentBranchHandler _nonPermanentBranch;
        public NonPermanentBranchController(INonPermanentBranchHandler nonPremanentBranch)
        {
            _nonPermanentBranch = nonPremanentBranch;
            _nonPermanentBranch.NotNull(nameof(_nonPermanentBranch));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(NonPermanentBranchInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchDataOutputDto> nonPremanentBranch = await _nonPermanentBranch.Handle(inputDto, cancellationToken);
            return Ok(nonPremanentBranch);
        }
    }
}
