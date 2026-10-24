using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.NotificationPool.Domain.Features.Sms;
using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;

namespace Aban360.Api.Controllers.V1.NotificationPool.Commands
{
    [Route("v1/bill")]
    public class BillSendLatestSmsController : BaseController
    {
        private readonly ILatestBillSendSmsHandler _billSendSmsHandler;
        public BillSendLatestSmsController(ILatestBillSendSmsHandler billSendSmsHandler)
        {
            _billSendSmsHandler = billSendSmsHandler;
            _billSendSmsHandler.NotNull(nameof(billSendSmsHandler));
        }

        [HttpPost]
        [Route("send-sms")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SearchInput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBillId(LatestBillSendDto input, CancellationToken cancellationToken)
        {
            await _billSendSmsHandler.Handle(input.BillId, input.Mobile, CurrentUser, cancellationToken);
            return Ok(input);
        }
    }
}
