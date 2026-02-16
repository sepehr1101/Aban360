using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.Usp.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.Usp.Input;
using Aban360.ReportPool.Domain.Features.Usp.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.Usp
{
    [Route("v1/usp")]
    public class UspPayment2Controller : BaseController
    {
        private readonly IUspPayment2Handler _handler;
        private readonly IReportGenerator _reportGenerator;

        public UspPayment2Controller(
            IUspPayment2Handler handler,
            IReportGenerator reportGenerator)
        {
            _handler = handler;
            _handler.NotNull(nameof(handler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(reportGenerator));  
        }
       
        [HttpPost]
        [Route("payment2/raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UspPayment2Output>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] UspPayment2Input input, CancellationToken cancellationToken)
        {            
            ReportOutput<UspPayment2Header, UspPayment2Output> output = await _handler.Handle(input, cancellationToken);
            return Ok(output);
        }

        [HttpPost]
        [Route("payment2/excel/{connectionId}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<UspPayment2Output>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcel(string connectionId,[FromBody] UspPayment2Input input, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(input, cancellationToken, _handler.Handle, CurrentUser, ReportLiterals.UspFinancial2, connectionId);
            return Ok(input);
        }
    }
}
