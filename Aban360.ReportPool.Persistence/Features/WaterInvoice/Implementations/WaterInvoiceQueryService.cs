using Aban360.ReportPool.Domain.Features.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;

namespace Aban360.ReportPool.Persistence.Features.WaterInvoice.Implementations
{
    internal class WaterInvoiceQueryService : IWaterInvoiceQueryService
    {
        public WaterInvoiceDto Get()
        {
            var waterInvoice = GetWaterInvoice();
            return waterInvoice;
        }

        private WaterInvoiceDto GetWaterInvoice()
        {
            var waterInvoice = new WaterInvoiceDto()
            {
                Headquarters = "شرکت آب و فاضلاب استان اصفهان",
                EconomicalNumber = "411-7676-4864",

                ZoneTitle = "منطقه پنج - 5",
                FullName = "سعید شمسایی",
                Address = "خیابان 17 شهریور-کوچه 13",
                PostalCode = "",

                UsageConsumptionTitle = "مسکونی",
                UsageSellTitle = "تجاری",

                UnitDomesticWater = 5,
                NoneDomestic = 0,
                EmptyUnit = 0,

                UsageId = 12,
                BodySerial = "150210",
                MeterDiameterTitle = "3/4",
                SiphonDiameterTitle = "125",
                CounterTitle = "عادی",

                ConsumerNumber = 55108696,
                ReadingNumber = "521904300",

                CurrentMeterDateJalali = "1403/06/07",
                PreviousMeterDateJalali = "1403/07/15",

                Duration = 39,

                CurrentMeterNumber = 8908,
                PreviousMeterNumber = 9005,

                ConsumptionM3 = 97,
                ConsumptionLiter = 97000,
                ConsumptionAverage = 14,

                LineItems = new List<LineItemsDto>()
                {
                    new LineItemsDto() {Item="آب بها",Amount=1825832},
                    new LineItemsDto() {Item="کارمزد دفع فاضلاب",Amount=1250882},
                    new LineItemsDto() {Item="مالیات و عوارض",Amount=1101968},
                    new LineItemsDto() {Item="تکالیف قانونی",Amount=1002258}
                },

                Sum = 1909832,
                DisCount = 0,
                DebtorOrCreditorAmount = -146,

                PayableAmount = 1909832,
                PaymentDeadline = "1403/07/28",

                ConsumptionState = 2,

                PreviousConsumptions = new List<PreviousConsumptionsDto>()
                {
                    new PreviousConsumptionsDto(){ ConsumptionAmount=10,ConsumptionDateJalali="1402/09016"},
                    new PreviousConsumptionsDto(){ ConsumptionAmount=10,ConsumptionDateJalali="1402/10/25"},
                    new PreviousConsumptionsDto(){ ConsumptionAmount=14,ConsumptionDateJalali="1402/12/02"},
                    new PreviousConsumptionsDto(){ ConsumptionAmount=11,ConsumptionDateJalali="1403/01/11"},
                    new PreviousConsumptionsDto(){ ConsumptionAmount=10,ConsumptionDateJalali="1403/02/18"},
                    new PreviousConsumptionsDto(){ ConsumptionAmount=16,ConsumptionDateJalali="1403/05/04"},
                    new PreviousConsumptionsDto(){ ConsumptionAmount=12,ConsumptionDateJalali="1403/06/06"},
                    new PreviousConsumptionsDto(){ ConsumptionAmount=12,ConsumptionDateJalali="1403/06/15"},
                },

                BillId = "2250932816311",
                PayId = "627110865",

                BarCode = "15236200102510141123959102",

                PaymenetAmountText = "یک میلیون و نه صد و  نه هزار و هشتصد و سی و دو",

                IsPayed = true,
                Description = "قبض پرداخت شده است",
                PaymentDateJalali = "1403/10/10",
                PaymentMethod = "اینترنت",
            };
            return waterInvoice;
        }
    }
}
