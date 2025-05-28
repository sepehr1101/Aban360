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
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=1000528958,
                    creditAmount=0,
                    Remained="1000528958",
                    Description="خوداظهاری",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000
                },new EventsSummaryDto()
                {
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=0,
                    creditAmount=1000528958,
                    Remained="411",
                    Description="تصویه حساب",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000

                },new EventsSummaryDto()
                {
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=1000528958,
                    creditAmount=0,
                    Remained="1000528958",
                    Description="قرائت",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000
                },new EventsSummaryDto()
                {
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=0,
                    creditAmount=1000528958,
                    Remained="411",
                    Description="خراب",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000
                },new EventsSummaryDto()
                {
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=1000528958,
                    creditAmount=0,
                    Remained="1000528958",
                    Description="قبض باطل شده",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000
                },new EventsSummaryDto()
                {
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=0,
                    creditAmount=1000528958,
                    Remained="411",
                    Description="برگشتی",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000
                },new EventsSummaryDto()
                {
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=1000528958,
                    creditAmount=0,
                    Remained="1000528958",
                    Description="برآوردی",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000
                },new EventsSummaryDto()
                {
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=0,
                    creditAmount=1000528958,
                    Remained="411",
                    Description="لوله ترکیدگی",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000
                },new EventsSummaryDto()
                {
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=1000528958,
                    creditAmount=0,
                    Remained="1000528958",
                    Description="صدور قبض",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000
                },new EventsSummaryDto()
                {
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=0,
                    creditAmount=1000528958,
                    Remained="411",
                    Description="قرائت",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000
                },new EventsSummaryDto()
                {
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=1000528958,
                    creditAmount=0,
                    Remained="1000528958",
                    Description="صدور قبض",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000
                },new EventsSummaryDto()
                {
                    CommercialUsage=5 ,
                    ResidentialUsage=2 ,
                    OtherUsage= 5,
                    HouseholderNumber   =0 ,
                    EmptyUnit= 0,
                    ContractualCapacity=15,
                    Usage=1,
                    ReadingNumber="15269",
                    Consumption="24",
                    ConsumptionAverage="14.5",
                    ReadingDate="1401/01/01",
                    DebtAmount=0,
                    creditAmount=1000528958,
                    Remained="411",
                    Description="صدور قبض",
                    BankTitle="پاسارگاد",
                    RegisterDate="1403/05/03",
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=15200,
                    NextMeterNumber=15000
                },

            };
        }
    }
}
/*
 * new EventsSummaryDto()
                {
                    CurrentMeterDate="1403/05/01",
                    DebtAmount= 6500,
                    Description="صدور قبض",
                    Id=1,
                    NextMeterNumber=9745,
                    CreditAmount=14055000,
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
                    CreditAmount=6500,
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
                    CreditAmount=14055000,
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=9745,
                    RegisterDate="1403/07/03",
                    Style="bank-payment",
                    BankTitle="صادرات",
                    ConsumptionAverage=null
                },
                new EventsSummaryDto()
                {
                    CurrentMeterDate="1404/03/01",
                    DebtAmount= 350000,
                    Description="پرداخت",
                    Id=3,
                    NextMeterNumber=null,
                    CreditAmount=14055000,
                    PreviousMeterDate="1404/01/15",
                    PreviousMeterNumber=9745,
                    RegisterDate="1404/01/16",
                    Style="bank-payment",
                    BankTitle="صادرات",
                    ConsumptionAverage=null
                },
                new EventsSummaryDto()
                {
                    CurrentMeterDate="1404/01/15",
                    DebtAmount= 350000,
                    Description="پرداخت",
                    Id=3,
                    NextMeterNumber=null,
                    CreditAmount=14055000,
                    PreviousMeterDate="1403/10/20",
                    PreviousMeterNumber=9745,
                    RegisterDate="1403/10/21",
                    Style="bank-payment",
                    BankTitle="صادرات",
                    ConsumptionAverage=null
                },
                new EventsSummaryDto()
                {
                    CurrentMeterDate="1403/10/20",
                    DebtAmount= 350000,
                    Description="پرداخت",
                    Id=3,
                    NextMeterNumber=null,
                    CreditAmount=14055000,
                    PreviousMeterDate="1403/07/02",
                    PreviousMeterNumber=9745,
                    RegisterDate="1403/07/03",
                    Style="bank-payment",
                    BankTitle="صادرات",
                    ConsumptionAverage=null
                },
 */