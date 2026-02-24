using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
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
            : base(configuration)
        { 
        }

        public WaterInvoiceDto Get()
        {
            WaterInvoiceDto waterInvoice = GetWaterInvoice();
            return waterInvoice;
        }
        public async Task<ReportOutput<WaterInvoiceDto, LineItemsDto>> Get(string billId)
        {
            string getWaterInvoiceQuery = GetWaterInvoiceQuery();
            string getItemValueQuery = GetItemsQuery();
            string getPreviousConsumptionQuery = GetPreviousConsumptionQuery();
            string getHeadquarterQuery = GetHeadquarterQuery();
            string getPaymentQuery = GetPaymentInfoQuery();

            WaterInvoiceDto waterInvoice = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterInvoiceDto>(getWaterInvoiceQuery, new { billId = billId });
            if (waterInvoice == null)
            {
                throw new BaseException(ExceptionLiterals.NotFoundAnyData);
            }
            IEnumerable<LineItemsDto> lineitems = await _sqlReportConnection.QueryAsync<LineItemsDto>(getItemValueQuery, new { billId = billId });
            IEnumerable<PreviousConsumptionsDto> previousConsumptions = await _sqlReportConnection.QueryAsync<PreviousConsumptionsDto>(getPreviousConsumptionQuery, new { billId = billId });
            string headquarterTitle = await _sqlConnection.QueryFirstAsync<string>(getHeadquarterQuery, new { zoneId = waterInvoice.ZoneId });
            WaterInvoicePaymentOutputDto? paymentInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterInvoicePaymentOutputDto>(getPaymentQuery, new { billId = billId, payId = waterInvoice.PayId == null ? "0" : waterInvoice.PayId, billRegisterDate = waterInvoice.RegisterDateJalali });
            waterInvoice.DebtorOrCreditorAmount = await GetRemained(billId);
            waterInvoice = MappingWaterInvoice(waterInvoice, paymentInfo, previousConsumptions, lineitems, headquarterTitle);

            ReportOutput<WaterInvoiceDto, LineItemsDto> result = new(ReportLiterals.WaterInvoice, waterInvoice, lineitems);

            return result;
        }
        public async Task<ReportOutput<WaterInvoiceDto, LineItemsDto>> Get(string billId, long id)
        {
            string IdPart = $" AND b.id={id} ";
            string getWaterInvoiceQuery = GetWaterInvoiceQuery(IdPart);
            string getItemValueQuery = GetItemsQuery();
            string getPreviousConsumptionQuery = GetPreviousConsumptionQuery();
            string getHeadquarterQuery = GetHeadquarterQuery();
            string getPaymentQuery = GetPaymentInfoQuery();

            WaterInvoiceDto waterInvoice = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterInvoiceDto>(getWaterInvoiceQuery, new { billId = billId });
            if (waterInvoice == null)
            {
                throw new BaseException(ExceptionLiterals.NotFoundAnyData);
            }
            IEnumerable<LineItemsDto> lineitems = await _sqlReportConnection.QueryAsync<LineItemsDto>(getItemValueQuery, new { billId = billId });
            IEnumerable<PreviousConsumptionsDto> previousConsumptions = await _sqlReportConnection.QueryAsync<PreviousConsumptionsDto>(getPreviousConsumptionQuery, new { billId = billId });
            string headquarterTitle = await _sqlConnection.QueryFirstAsync<string>(getHeadquarterQuery, new { zoneId = waterInvoice.ZoneId });
            WaterInvoicePaymentOutputDto? paymentInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterInvoicePaymentOutputDto>(getPaymentQuery, new { billId = billId, payId = waterInvoice.PayId == null ? "0" : waterInvoice.PayId, billRegisterDate = waterInvoice.RegisterDateJalali });
            waterInvoice.DebtorOrCreditorAmount = await GetRemained(billId);
            waterInvoice = MappingWaterInvoice(waterInvoice, paymentInfo, previousConsumptions, lineitems, headquarterTitle);

            ReportOutput<WaterInvoiceDto, LineItemsDto> result = new(ReportLiterals.WaterInvoice, waterInvoice, lineitems);

            return result;
        }
        public async Task<int> GetOlgo(int zoneId)
        {
            string getOlgoQuery = GetOlgoQuery();
            int olgo = await _sqlReportConnection.QueryFirstOrDefaultAsync<int>(getOlgoQuery, new { zoneId = zoneId });
            return olgo;
        }
        private WaterInvoiceDto MappingWaterInvoice(WaterInvoiceDto waterInvoice, WaterInvoicePaymentOutputDto? paymentInfo, IEnumerable<PreviousConsumptionsDto> previousConsumptions, IEnumerable<LineItemsDto> lineitems, string headquarterTitle)
        {
            //waterInvoice.LineItems = lineitems.ToList();
            waterInvoice.PreviousConsumptions = previousConsumptions.ToList();
            waterInvoice.Sum = lineitems.Select(i => i.Amount).Sum();
            waterInvoice.Headquarters = headquarterTitle;

            waterInvoice.PaymentDateJalali = paymentInfo is not null ? paymentInfo.PaymentDateJalali : "";
            waterInvoice.PaymentMethod = paymentInfo is not null ? paymentInfo.PaymentMethod : "";
            waterInvoice.IsPayed = paymentInfo is not null;
            waterInvoice.Description = paymentInfo != null ? ExceptionLiterals.SuccessedPay : ExceptionLiterals.UnsuccessedPay;
            waterInvoice.Title = ReportLiterals.WaterInvoice;

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
                        b.ConsumptionAverage AS ConsumptionAmount,
			            b.RegisterDay AS ConsumptionDateJalali
	                  From [CustomerWarehouse].dbo.Bills b
	                  WHERE
	                 	b.BillId=@billId AND
                        b.CounterStateCode NOT IN (4,7,8)
	                 order by PreviousDay desc";
        }
        private string GetWaterInvoiceQuery(string idPart="")
        {
            return @$"Select top 1
                    	N'شرکت آب و فاضلاب استان اصفهان' AS Headquarters,
                    	'411-7676-4864' AS EconomicalNumber,--todo
                    	b.ZoneTitle AS ZoneTitle,
                        b.ZoneId AS ZoneId,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) AS SurName,
                    	(TRIM(c.FirstName) + ' ' + TRIM(c.SureName)) AS FullName,
                    	TRIM(c.Address) AS Address,
                    	TRIM(c.PostalCode) AS PostalCode,
                    	b.UsageTitle AS UsageSellTitle,
                    	b.UsageTitle2 AS UsageConsumptionTitle,
						b.BranchType AS UseStateTitle,
                    	b.DomesticCount AS UnitDomesticWater,
                    	b.CommercialCount+b.OtherCount AS NoneDomestic,
                    	b.EmptyCount AS EmptyUnit,
                    	b.UsageId AS UsageId,
                    	TRIM(c.MeterSerialBody)  AS BodySerial,
                    	b.WaterDiameterId AS MeterDiameterId,
                    	b.WaterDiameterTitle AS MeterDiameterTitle,
                    	b.CounterStateTitle AS CounterTitle,
						c.MainSiphonTitle AS SiphonDiameterTitle,
                    	b.CustomerNumber AS ConsumerNumber,
                    	b.ReadingNumber AS ReadingNumber,
                    	b.NextDay AS CurrentMeterDateJalali,
                    	b.PreviousDay AS PreviousMeterDateJalali,
                    	--Duration Calc in c#
						b.NextNumber AS CurrentMeterNumber,
						b.PreviousNumber AS PreviousMeterNumber,
                    	b.Consumption AS ConsumptionM3,--todo
                    	(b.Consumption)*1000 AS ConsumptionLiter,
                    	b.ConsumptionAverage AS ConsumptionAverage,
						b.ContractCapacity AS ContractualCapacity,
                    	--LineItems in secord query
						b.SumItems AS Sum,
						(b.ItemOff1 + b.ItemOff2 + b.ItemOff3 + b.ItemOff4 + b.ItemOff5 + b.ItemOff6 + b.ItemOff7 + b.ItemOff8 + b.ItemOff9 + b.ItemOff10 + b.ItemOff11 + b.ItemOff12 + b.ItemOff13 + b.ItemOff14 + b.ItemOff15 + b.ItemOff16 + b.ItemOff17) AS DisCount,
						b.PreDebt AS DebtorOrCreditorAmount,
                    	b.Payable AS PayableAmount,
                    	b.Deadline AS PaymentDeadline,
                        b.RegisterDay AS RegisterDateJalali,
                    	--consumptionState in c#
                    	--previousConsumption in secod query
                    	TRIM(b.BillId) AS BillId,
                    	TRIM(b.PayId) AS PayId,
                    	N'--' AS PaymentAmountText,
                    	1 As IsPayed,--todo
                    	N'--' AS Description,
                    	'-' AS PaymentDateJalali,--todo,
                    	'-' AS PaymentMethod--todo
                    From [CustomerWarehouse].dbo.Bills b
                    join [CustomerWarehouse].dbo.Clients c on b.BillId=c.BillId
                    Where 
                        c.ToDayJalali IS NULL AND
                    	b.BillId=@billId AND
                        b.CounterStateCode NOT IN (4,7) And
						b.TypeId In (N'قبض', N'علی الحساب') {idPart} 
                    order by PreviousDay desc";
        }

        private string GetItemsQuery()
        {
            return @"SELECT v.Item, v.Amount
                     FROM (
                         SELECT TOP 1 *
                         FROM [CustomerWarehouse].dbo.Bills
                         WHERE BillId = @billId  AND TypeId IN (N'قبض', N'علی الحساب')
                         ORDER BY PreviousDay DESC
                     ) b
                     CROSS APPLY (
                         VALUES
                             (N'بهای آب مصرفی', b.Item1 + b.Item3 + b.Item7 + b.Item9 + b.Item8 + b.Item11 + b.Item12 + b.Item13 + b.Item6),
                             (N'بهای کارمزد دفع فاضلاب', b.Item2 + b.Item4 + b.Item14 + b.Item15),
                             (N'مالیات',b.Item5),
                             (N'تکالیف قانونی',b.Item10 + b.Item16 + b.Item17),
                             (N'تخفیف',               
                                 ISNULL(b.ItemOff1, 0) + ISNULL(b.ItemOff2, 0) + ISNULL(b.ItemOff3, 0) + ISNULL(b.ItemOff4, 0) +
                                 ISNULL(b.ItemOff5, 0) + ISNULL(b.ItemOff6, 0) + ISNULL(b.ItemOff7, 0) + ISNULL(b.ItemOff8, 0) +
                                 ISNULL(b.ItemOff9, 0) + ISNULL(b.ItemOff10, 0) + ISNULL(b.ItemOff11, 0) + ISNULL(b.ItemOff12, 0) +
                                 ISNULL(b.ItemOff13, 0) + ISNULL(b.ItemOff14, 0) + ISNULL(b.ItemOff15, 0) + ISNULL(b.ItemOff16, 0) +
                                 ISNULL(b.ItemOff17, 0)
                             )
                     ) v(Item, Amount)";
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

                //LineItems = new List<LineItemsDto>()
                //{
                //    new LineItemsDto() {Item="آب بها",Amount=1825832},
                //    new LineItemsDto() {Item="کارمزد دفع فاضلاب",Amount=1250882},
                //    new LineItemsDto() {Item="مالیات و عوارض",Amount=1101968},
                //    new LineItemsDto() {Item="تکالیف قانونی",Amount=1002258}
                //},

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
        private string GetPaymentInfoQuery()
        {
            return @"Select 
                    	p.PaymentGateway AS PaymentMethod,
                    	p.RegisterDay AS PaymentDateJalali
                    From [CustomerWarehouse].dbo.Payments p
                    Where 
                    	p.BillId=@billId AND
                    	p.PayId=@payId AND
                        p.PayDateJalali >=@billRegisterDate
                    Order By
                    	p.RegisterDay Desc";
        }
        private string GetOlgoQuery()
        {
            return @"Select Olgo
                    from [OldCalc].dbo.Table1
                    Where Town=@zoneId";
        }
        private string GetDebtorAndCreditorQuery()
        {
            return @"select   
                    	SumItems DebtAmount,
                         0 CreditAmount,
                        TypeCode,
                        RegisterDay RegisterDate
                    from [CustomerWarehouse].dbo.Bills
                     where 
                        (BillId)=@billId
                    union
                     select
                    	0 DebtAmount, 
                         Amount CreditAmount,
                        0 TypeCode,
                        RegisterDay RegisterDate
                    from [CustomerWarehouse].dbo.Payments
                     where 
                        (BillId)=@billId ";
        }
        private async Task<long> GetRemained(string billId)
        {
            string getDebtorAndCreditorQuery = GetDebtorAndCreditorQuery();
            IEnumerable<DebtorAndCreaditorOutputDto> debtorAndCreditor = await _sqlReportConnection.QueryAsync<DebtorAndCreaditorOutputDto>(getDebtorAndCreditorQuery, new { billId = billId });

            long lastRemained = 0;
            if (debtorAndCreditor is not null && debtorAndCreditor.Any())
            {
                debtorAndCreditor = debtorAndCreditor.OrderBy(i => i.RegisterDate);

                for (int i = 0; i < debtorAndCreditor.Count(); i++)
                {
                    DebtorAndCreaditorOutputDto row = debtorAndCreditor.ElementAt(i);
                    if (row.TypeCode == 7)
                    {
                        continue;
                    }
                    lastRemained = lastRemained + (row.DebtAmount.Value - row.CreditAmount);
                }
            }

            return lastRemained;
        }
    }
}
