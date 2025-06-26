using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.WaterInvoice.Implementations
{
    internal class WaterInvoiceQueryService : AbstractBaseConnection, IWaterInvoiceQueryService
    {
        public WaterInvoiceQueryService(IConfiguration configuration)
            : base(configuration) { }

        public WaterInvoiceDto Get()
        {
            WaterInvoiceDto waterInvoice = GetWaterInvoice();
            return waterInvoice;
        }
        public async Task<WaterInvoiceDto> Get(string billId)
        {
            string getWaterInvoiceQuery = GetWaterInvoiceQuery();
            string getItemValueQuery = GetItemValueQuery();
            string getPreviousConsumptionQuery = GetPreviousConsumptionQuery();
            string getHeadquarterQuery= GetHeadquarterQuery();

            WaterInvoiceDto waterInvoice = await _sqlReportConnection.QueryFirstAsync<WaterInvoiceDto>(getWaterInvoiceQuery, new { billId = billId });
            IEnumerable<LineItemsDto> lineitems = await _sqlReportConnection.QueryAsync<LineItemsDto>(GetItemValueQuery(), new { billId = billId });
            IEnumerable<PreviousConsumptionsDto> previousConsumptions = await _sqlReportConnection.QueryAsync<PreviousConsumptionsDto>(getPreviousConsumptionQuery, new { billId = billId });
            string headquarterTitle = await _sqlReportConnection.QueryFirstAsync<string>(GetHeadquarterQuery(), new { zoneId = waterInvoice.ZoneId });
          
            waterInvoice.LineItems = lineitems.ToList();
            waterInvoice.PreviousConsumptions = previousConsumptions.ToList();
            waterInvoice.Sum=lineitems.Select(i=> i.Amount).Sum();
            waterInvoice.Headquarters = headquarterTitle;
            waterInvoice.Duration = Convert.ToInt16((Convert.ToDateTime(waterInvoice.CurrentMeterDateJalali) - Convert.ToDateTime(waterInvoice.PreviousMeterDateJalali)).Days);


            return waterInvoice;
        }

        private string GetHeadquarterQuery()
        {
            return @"select h.Title
	                 from [Aban360].LocationPool.Zone z
	                 join [Aban360].LocationPool.Region r on z.RegionId=r.Id 
	                 join [Aban360].LocationPool.Headquarters h on r.HeadquartersId=h.Id
	                 where z.Id=@zoneId";
        }
        private string GetPreviousConsumptionQuery()
        {
            return @"Select top 10
                        b.Payable AS ConsumptionAmount,
			            b.RegisterDay AS ConsumptionDateJalali
	                  From [CustomerWarehouse].dbo.Bills b
	                  WHERE
	                 	b.BillId=@billId 
	                 order by PreviousDay desc";
        }

        private string GetItemValueQuery()
        {
            return @"SELECT
                		v.Value AS Amount,
                		o.Title AS Item
                	FROM [CustomerWarehouse].dbo.Bills b
                	CROSS APPLY (
                		VALUES
                			('Item1', b.Item1, 78),
                			('Item2', b.Item2, 79),
                			('Item3', b.Item3, 80),
                			('Item4', b.Item4, 81),
                			('Item5', b.Item5, 82),
                			('Item6', b.Item6, 83),
                			('Item7', b.Item7, 84),
                			('Item8', b.Item8, 85),
                			('Item9', b.Item9, 86),
                			('Item10', b.Item10, 87),
                			('Item11', b.Item11, 88),
                			('Item12', b.Item12, 89),
                			('Item13', b.Item13, 90),
                			('Item14', b.Item14, 91),
                			('Item15', b.Item15, 92),
                			('Item16', b.Item16, 93),
                			('Item17', b.Item17, 94),
                			('ItemOff1', b.ItemOff1, 95),
                			('ItemOff2', b.ItemOff2, 96),
                			('ItemOff3', b.ItemOff3, 97),
                			('ItemOff4', b.ItemOff4, 98),
                			('ItemOff5', b.ItemOff5, 99),
                			('ItemOff6', b.ItemOff6, 100),
                			('ItemOff7', b.ItemOff7, 101),
                			('ItemOff8', b.ItemOff8, 102),
                			('ItemOff9', b.ItemOff9, 103),
                			('ItemOff10', b.ItemOff10, 104),
                			('ItemOff11', b.ItemOff11, 105),
                			('ItemOff12', b.ItemOff12, 106),
                			('ItemOff13', b.ItemOff13, 107),
                			('ItemOff14', b.ItemOff14, 108),
                			('ItemOff15', b.ItemOff15, 109),
                			('ItemOff16', b.ItemOff16, 110),
                			('ItemOff17', b.ItemOff17, 111)
                			-- و ...
                	) v(ItemName, Value, OfferingId)
                	INNER JOIN [Aban360].CalculationPool.Offering o ON o.Id = v.OfferingId
                	WHERE
                		v.Value > 0 AND
                		b.BillId=@billId AND
                		b.NextNumber=0
                	order by PreviousDay desc";
        }
        private string GetWaterInvoiceQuery()
        {
            return @"Select top 1
                    	N'شرکت آب و فاضلاب استان اصفهان' AS Headquarters,
                    	411-7676-4864 AS EconomicalNumber,--todo
                    	b.ZoneTitle AS ZoneTitle,
                        b.ZoneId AS ZoneId,
                    	c.FirstName AS FirstName,
                    	c.SureName AS SurName,
                    	c.FirstName + ' ' + c.SureName AS FullName,
                    	c.Address AS Address,
                    	c.PostalCode AS PostalCode,
                    	c.UsageTitle AS UsageSellTitle,
                    	c.UsageTitle2 AS UsageConsumptionTitle,
                    	c.DomesticCount AS UnitDomesticWater,
                    	0 AS NoneDomestic,--todo
                    	c.EmptyCount AS EmptyUnit,
                    	c.UsageId AS UsageId,
                    	c.MeterSerialBody AS BodySerial,
                    	c.WaterDiameterId AS MeterDiameterId,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	b.CounterStateTitle AS CounterTitle,
                    	'123456789' AS ConsumerNumber,--todo
                    	c.ReadingNumber AS ReadingNumber,
                    	b.RegisterDay AS CurrentMeterDateJalali,--todo
                    	b.PreviousDay AS PreviousMeterDateJalali,
                    	--Duration Calc in c#
                    	b.NextNumber-b.PreviousNumber AS CurrentMeterNubmer,--todo
                    	b.PreviousNumber AS PreviousMeterNumber,
                    	b.Consumption AS ConsumptionM3,--todo
                    	b.Consumption AS ConsumptionLiter,--todo
                    	b.ConsumptionAverage AS ConsumptionAverage,
                    	--LineItems in secord query
                    	--sum/discount/debtororcreditoramount in c#
                    	b.Payable AS PayableAmount,
                    	b.Deadline AS PaymentDeadline,
                    	--consumptionState in c#
                    	--previousConsumption in secod query
                    	b.BillId AS BillId,
                    	'123456789' AS PayId,--todo
                    	'15236200102510141123959102' AS BarCode,
                    	N'--' AS PaymentAmountText,
                    	1 As IsPayed,--todo
                    	N'--' AS Description,
                    	'-' AS PaymentDateJalali,--todo,
                    	'-' AS PaymentMethod--todo
                    From [CustomerWarehouse].dbo.Bills b
                    join [CustomerWarehouse].dbo.Clients c on b.BillId=c.BillId
                    Where 
                    	b.BillId=@billId AND
                    	b.NextNumber=0
                    order by PreviousDay desc";
        }

        private WaterInvoiceDto GetWaterInvoice()
        {
            WaterInvoiceDto waterInvoice = new WaterInvoiceDto()
            {
                Headquarters = "شرکت آب و فاضلاب استان اصفهان",
                EconomicalNumber = "411-7676-4864",

                ZoneTitle = "منطقه پنج - 5",
                FullName = "حمیدرضا رضایی",
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
