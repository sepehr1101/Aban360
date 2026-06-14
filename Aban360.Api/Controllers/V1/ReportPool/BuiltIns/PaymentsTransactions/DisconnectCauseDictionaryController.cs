using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/disconnect-cause")]
    public class DisconnectCauseDictionaryController : BaseController
    {
        private readonly IConnectDisconnectCommandHandler _connectDisconnectPrintHandler;
        public DisconnectCauseDictionaryController(IConnectDisconnectCommandHandler connectDisconnectPrintHandler)
        {
            _connectDisconnectPrintHandler = connectDisconnectPrintHandler;
            _connectDisconnectPrintHandler.NotNull(nameof(connectDisconnectPrintHandler));
        }

        [HttpGet]
        [Route("get-dictionary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<NumericDictionary>>), StatusCodes.Status200OK)]
        public IActionResult GetDictionary(CancellationToken cancellationToken)
        {
            var causes = _connectDisconnectPrintHandler.GetCasues();
            return Ok(causes);
        }
    }
}
