using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.ConsumersInfo
{
    [Route("v1/subscription")]
    [ApiController]
    public class SubscriptionEventsSummaryInfoController : BaseController
    {
        private readonly ISubscriptionEventQueryService _subscriptionEventQueryService;
        public SubscriptionEventsSummaryInfoController(
            ISubscriptionEventQueryService subscriptionEventQueryService)
        {
            _subscriptionEventQueryService = subscriptionEventQueryService;
            _subscriptionEventQueryService.NotNull(nameof(subscriptionEventQueryService));
        }

        [HttpPost]
        [Route("events-summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EventsSummaryDto>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetEventsSummaryInfo([FromBody] SearchInput searchInput)
        {
            try
            {
                IEnumerable<EventsSummaryDto> items = await _subscriptionEventQueryService.GetEventsSummaryDtos(searchInput.Input);
                return Ok(items);
            }
            catch
            {
                return Ok(GetEventsSummaryDtos());
            }
        }
        private ICollection<EventsSummaryDto> GetEventsSummaryDtos()
        {
            return new List<EventsSummaryDto>()
            {
                new EventsSummaryDto()
                {
                    CurrentMeterDate="1403/05/01",
                    DebtAmount= 6500,
                    Description="صدور قبض",
                    Id=1,
                    NextMeterNumber=9745,
                    OweAmount=14055000,
                    PreviousMeterDate="1403/01/15",
                    PreviousMeterNumber=9354,
                    RegisterDate="1403/05/03",
                    Style="water-meter-icon",
                    BankTitle=null,
                    ConsumptionAverage=41.25f
                },
                new EventsSummaryDto()
                {
                    CurrentMeterDate="1403/05/04",
                    DebtAmount= 14055000,
                    Description="صدور قبض",
                    Id=2,
                    NextMeterNumber=9850,
                    OweAmount=6500,
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=9745,
                    RegisterDate="1403/07/03",
                    Style="water-meter-icon",
                    BankTitle=null,
                    ConsumptionAverage=35.15f
                },
                 new EventsSummaryDto()
                {
                    CurrentMeterDate="1403/05/04",
                    DebtAmount= 8400,
                    Description="پرداخت",
                    Id=3,
                    NextMeterNumber=null,
                    OweAmount=14055000,
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=9745,
                    RegisterDate="1403/07/03",
                    Style="bank-payment",
                    BankTitle="صادرات",
                    ConsumptionAverage=null
                },
            };
        }
    }
}
