using Aban360.Common.Categories.ApiResponse;
using Aban360.ReportPool.Domain.Features.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool
{
    [Route("v1/subscription")]
    [ApiController]
    public class SubscriptionEventsSummaryInfoController : BaseController
    {

        [HttpPost]
        [Route("events-summary")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EventsSummaryDto>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetEventsSummaryInfo([FromBody] SearchInput searchInput)
        {            
            return Ok(GetEventsSummaryDtos());
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
    public record EventsSummaryDto
    {
        public int Id { get; set; }
        public int? PreviousMeterNumber { get; set; }
        public int? NextMeterNumber { get; set; }
        public string? Description { get; set; }
        public string Style { get; set; } = default!;
        public long? DebtAmount { get; set; }
        public long? OweAmount { get; set; }
        public string? PreviousMeterDate { get; set; }
        public string? CurrentMeterDate { get; set; }
        public string RegisterDate { get; set; } = default!;
        public float? ConsumptionAverage { get; set; }
        public string? BankTitle { get; set; }
    }
}
