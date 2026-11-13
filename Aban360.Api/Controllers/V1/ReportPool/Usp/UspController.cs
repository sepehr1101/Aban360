using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Usp.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Usp.Input;
using Aban360.ReportPool.Domain.Features.Usp.Output;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.ReportPool.Usp
{
    public class UspController : BaseController
    {
        private readonly IUspFinancial2Handler _c0_handler;
        private readonly IReportGenerator _c0_reportGenerator;
        private readonly IUspPayment2Handler _c1_handler;
        private readonly IReportGenerator _c1_reportGenerator;
        public UspController(IUspFinancial2Handler c0_handler, IReportGenerator c0_reportGenerator, IUspPayment2Handler c1_handler, IReportGenerator c1_reportGenerator)
        {
            _c0_handler = c0_handler;
            _c0_handler.NotNull(nameof(c0_handler));
            _c0_reportGenerator = c0_reportGenerator;
            _c0_reportGenerator.NotNull(nameof(c0_reportGenerator));
            _c1_handler = c1_handler;
            _c1_handler.NotNull(nameof(c1_handler));
            _c1_reportGenerator = c1_reportGenerator;
            _c1_reportGenerator.NotNull(nameof(c1_reportGenerator));
        }

        [HttpPost]
        [Route("~/v1/usp/financial2")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UspFinancial2Output>>), StatusCodes.Status200OK)]
        [ActionName("Get")]
        public async Task<IActionResult> GetFromUspFinancial20([FromBody] UspFinancial2Input input, CancellationToken cancellationToken)
        {
            ReportOutput<UspFinancial2Header, UspFinancial2Output> output = await _c0_handler.Handle(input, cancellationToken);
            return Ok(output);
        }

        [HttpPost]
        [Route("~/v1/usp/financial2/excel/{connectionId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UspFinancial2Output>>), StatusCodes.Status200OK)]
        [ActionName("GetExcel")]
        public async Task<IActionResult> GetExcelFromUspFinancial20(string connectionId, [FromBody] UspFinancial2Input input, CancellationToken cancellationToken)
        {
            await _c0_reportGenerator.FireAndInform(input, cancellationToken, _c0_handler.Handle, CurrentUser, ReportLiterals.UspFinancial2, connectionId);
            return Ok(input);
        }

        [HttpPost]
        [Route("~/v1/usp/payment2/raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UspPayment2Output>>), StatusCodes.Status200OK)]
        [ActionName("Get")]
        public async Task<IActionResult> GetFromUspPayment21([FromBody] UspPayment2Input input, CancellationToken cancellationToken)
        {
            ReportOutput<UspPayment2Header, UspPayment2Output> output = await _c1_handler.Handle(input, cancellationToken);
            return Ok(output);
        }

        [HttpPost]
        [Route("~/v1/usp/payment2/excel/{connectionId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UspPayment2Output>>), StatusCodes.Status200OK)]
        [ActionName("GetExcel")]
        public async Task<IActionResult> GetExcelFromUspPayment21(string connectionId, [FromBody] UspPayment2Input input, CancellationToken cancellationToken)
        {
            await _c1_reportGenerator.FireAndInform(input, cancellationToken, _c1_handler.Handle, CurrentUser, ReportLiterals.UspFinancial2, connectionId);
            return Ok(input);
        }
    }
}
