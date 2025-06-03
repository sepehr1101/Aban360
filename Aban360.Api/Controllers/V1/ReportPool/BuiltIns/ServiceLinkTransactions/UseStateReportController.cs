using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/use-state-report")]
    public class UseStateReportController:BaseController
    {
        private readonly IUseStateReportHandler _useStateReportHandler;
        public UseStateReportController(IUseStateReportHandler useStateReportHandler)
        {
            _useStateReportHandler = useStateReportHandler;
            _useStateReportHandler.NotNull(nameof(_useStateReportHandler));
        }

        [HttpPost, HttpGet]
        [Route("info")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto>>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIfo(UseStateReportInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UseStateReportHeaderOutputDto, UseStateReportDataOutputDto> useStates =await _useStateReportHandler.Handle(inputDto,cancellationToken);
            return Ok(useStates);
        }
    }
}
