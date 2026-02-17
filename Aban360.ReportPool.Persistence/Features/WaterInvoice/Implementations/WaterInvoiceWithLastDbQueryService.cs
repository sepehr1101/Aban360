using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.WaterInvoice.Implementations
{
    internal sealed class WaterInvoiceWithLastDbQueryService : AbstractBaseConnection, IWaterInvoiceWithLastDbQueryService
    {
        public WaterInvoiceWithLastDbQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterInvoiceDto, LineItemsDto>> Get(ZoneIdAndCustomerNumberOutputDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            string getWaterInvoiceQuery = GetWaterInvoiceQuery(dbName);
            string getItemValueQuery = GetItemsQuery(dbName);
            string getPreviousConsumptionQuery = GetPreviousConsumptionQuery(dbName);
            string getHeadquarterQuery = GetHeadquarterQuery();
            string getPaymentQuery = GetPaymentInfoQuery(dbName);

            WaterInvoiceDto waterInvoice = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterInvoiceDto>(getWaterInvoiceQuery, input);
            if (waterInvoice == null)
            {
                throw new BaseException(ExceptionLiterals.NotFoundAnyData);
            }

            IEnumerable<LineItemsDto> lineitems = await _sqlReportConnection.QueryAsync<LineItemsDto>(getItemValueQuery, input);
            IEnumerable<PreviousConsumptionsDto> previousConsumptions = await _sqlReportConnection.QueryAsync<PreviousConsumptionsDto>(getPreviousConsumptionQuery, input);

            string headquarterTitle = await _sqlConnection.QueryFirstAsync<string>(getHeadquarterQuery, new { zoneId = waterInvoice.ZoneId });
            WaterInvoicePaymentOutputDto? paymentInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterInvoicePaymentOutputDto>(getPaymentQuery, new { billId = waterInvoice.BillId, payId = waterInvoice.PayId == null ? "0" : waterInvoice.PayId, billRegisterDate = waterInvoice.RegisterDateJalali, zoneId = waterInvoice.ZoneId, customerNumber = waterInvoice.ConsumerNumber });

           // waterInvoice.DebtorOrCreditorAmount = await GetRemained(input, dbName);
            waterInvoice = MappingWaterInvoice(waterInvoice, paymentInfo, previousConsumptions, lineitems, headquarterTitle);

            ReportOutput<WaterInvoiceDto, LineItemsDto> result = new(ReportLiterals.WaterInvoice, waterInvoice, lineitems);

            return result;

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
		#region CaclRemained
		//private async Task<long> GetRemained(ZoneIdAndCustomerNumberOutputDto input, string dbName)////invalid
		//{//--131701-100 invalid  --1000001816318-131301 invalid 
		//    string getDebtorAndCreditorQuery = GetDebtorAndCreditorQuery(dbName);
		//    IEnumerable<DebtorAndCreaditorOutputDto> debtorAndCreditor = await _sqlReportConnection.QueryAsync<DebtorAndCreaditorOutputDto>(getDebtorAndCreditorQuery, input);

		//    long lastRemained = 0;
		//    if (debtorAndCreditor is not null && debtorAndCreditor.Any())
		//    {
		//        debtorAndCreditor = debtorAndCreditor.OrderBy(i => i.RegisterDate);

		//        for (int i = 0; i < debtorAndCreditor.Count(); i++)
		//        {
		//            DebtorAndCreaditorOutputDto row = debtorAndCreditor.ElementAt(i);
		//            if (row.TypeCode == 7)
		//            {
		//                continue;
		//            }
		//            lastRemained = lastRemained + (row.DebtAmount.Value - row.CreditAmount);
		//        }
		//    }

		//    return lastRemained;
		//}
		//   private string GetDebtorAndCreditorQuery(string dbName)//todo: check prop
		//   {
		//       return $@"Select 
		//	b.baha DebtAmount,
		//	0 CreditAmount,
		//	b.type TypeCode,--todo:?
		//	b.date_bed RegisterDate
		//From [{dbName}].dbo.bed_bes b
		//Where
		//	b.town=@zoneId AND
		//	b.radif=@customerNumber AND
		//	b.date_bed >='1401/01/01'
		//Union 
		//Select 
		//	0 DebtAmount,
		//	v.pard CreditAmount,
		//	0 TypeCode,--todo:?
		//	v.date_sabt RegisterDate
		//From [{dbName}].dbo.vosolab v
		//Where
		//	v.town=@zoneId  AND
		//	v.radif=@customerNumber AND
		//	v.date_bank >='1401/01/01'";
		//   }
		#endregion
		private string GetWaterInvoiceQuery(string dbName)//todo: check Condition
        {
            return $@"Select Top 1
						b.Id,
						N'شرکت آب و فاضلاب استان اصفهان' Headquarters,
						'411-7676-4864' EconomicalNumber,
						t51.C2 ZoneTitle,
						m.town ZoneId,
						TRIM(m.name) FirstName,
						TRIM(m.family) Surname,
						(TRIM(m.name)+' '+TRIM(m.family)) FullName,
						TRIM(m.address) Address,
						TRIM(m.POST_COD) PostalCode,
						t41_sell.C1 UsageSellTitle,
						t41_consumption.C1 UsageConsumptionTitle,
						t7.C1 UseStateTitle,
						b.tedad_mas UnitDomesticWater,
						(b.tedad_tej+b.tedad_vahd) NoneDomestic,
						b.Khali_s EmptyUnit,
						b.cod_enshab UsageId,
						TRIM(m.serial_co) BodySerial,
						b.enshab MeterDiameterId,
						t5.C2 MeterDiameterTitle,
						b.cod_vas CounterId,
						cv.Title CounterTitle,
						Case When m.sif_1>0 Then N'قطر 100'
									 When m.sif_2>0 Then N'قطر 125'
									 When m.sif_3>0 Then N'قطر 150'
									 When m.sif_4>0 Then N'قطر 200'
									 When m.sif_5>0 Then N'قطر 5'
									 When m.sif_6>0 Then N'قطر 6'
									 When m.sif_7>0 Then N'قطر 7'
									 When m.sif_8>0 Then N'قطر 8'
									 Else N'ندارد'
						End as SiphonDiameterTitle,
						b.radif ConsumerNumber,
						TRIM(b.eshtrak) ReadingNumber,
						b.today_date CurrentMeterDateJalali,
						b.pri_date PreviousMeterDateJalali,
						--Duration Calc in c#
						b.today_no CurrentMeterNumber,
						b.pri_no PreviousMeterNumber,
						b.masraf ConsumptionM3,
						(b.masraf*1000) ConsumptionLiter,
						b.rate ConsumptionAverage,
						b.fix_mas ContractualCapacity,
						--LineItems in secord query
						b.baha Sum,
						b.kasr_ha DisCount,
						b.jam-b.baha DebtorOrCreditorAmount,
						b.pard PayableAmount,
						b.mohlat PaymentDeadline,
						b.date_bed RegisterDateJalali,
						--consumptionState in c#
						--previousConsumption in secod query
						TRIM(b.sh_ghabs1) BillId,
						TRIM(b.sh_pard1) PayId,
						'-' PaymentAmountText,
						1 IsPayed,
						'-' Description,
						'-' PaymentDateJalali,
						'-' PaymentMethod,
						IIF(b.mamor=888,N'خوداظهاری غیرحضوری',IIF(b.mamor=999,N'خوداظهاری حضوری',IIF(b.mamor=0,N'بدون کد مامور',N'دارای کد مامور'))) IssueType,
						b.operator ReaderName,
						IIF(b.date_bed>=v.date_check AND b.date_bed>FORMAT(DateAdd(DAY , -35 , GETDATE()),'yyyy/MM/dd','fa-ir'),1,0) IsRemovable
					From [{dbName}].dbo.members m
					Join [{dbName}].dbo.bed_bes b
						ON m.town=b.town AND m.radif=b.radif
					Join [Db70].dbo.T51 t51
						ON m.town=t51.C0
					Join [Db70].dbo.T41 t41_sell
						ON b.cod_enshab=t41_sell.C0
					Join [Db70].dbo.T41 t41_consumption
						ON b.group1=t41_consumption.C0
					Join [Db70].dbo.T7 t7
						ON b.noe_va=t7.C0
					Join [Db70].dbo.T5 t5
						ON b.enshab=t5.C0
					Join [Db70].dbo.CounterVaziat cv
						ON b.cod_vas=cv.MoshtarakinId
					Left Join [{dbName}].dbo.variab v
						ON m.town=v.town
					Where 
						m.town=@zoneId AND
						m.radif=@customerNumber AND 
						--b.type In ('1', '7') AND
						b.cod_vas NOT IN (4,7,8) 
					Order By b.date_bed DESC, id desc";
        }
        private string GetItemsQuery(string dbName)
        {
            return $@"Select v.Item , v.Amount
					From (
						 Select top 1 *
						 From [{dbName}].dbo.bed_bes b
						 Where 
						 	b.town=@zoneId AND
						 	b.radif=@customerNumber AND
						 	--b.TypeId In (N'قبض', N'علی الحساب') AND
						 	b.cod_vas NOT IN (4,7,8) 
						 Order By b.date_bed DESC, id desc
						 )b
					Cross Apply(
						Values
							(N'بهای آب مصرفی',b.ab_baha+b.abon_ab+0+0+b.jarime+b.zabresani+b.zaribfasl+b.ztadil+0),
							(N'بهای کارمزد دفع فاضلاب',b.fas_baha+b.abon_fas+b.TABS_FA+b.TAB_ABN_F),
							(N'مالیات',b.shahrdari),
							(N'تکالیف قانونی',b.zarib_d +0+b.bodjeh),
							(N'تخفیف',b.kasr_ha)
					) v(Item, Amount)";
        }
        private string GetPreviousConsumptionQuery(string dbName)
        {
            return $@"Select Top 10
						b.rate ConsumptionAmount,
						b.date_bed ConsumptionDateJalali
					From [{dbName}].dbo.bed_bes b
					Where 
						b.town=@zoneId AND
						b.radif=@customerNumber AND
						b.cod_vas NOT IN (4,7) 
					Order By b.pri_date Desc";
        }
        private string GetHeadquarterQuery()
        {
            return @"select h.Title
	                 from [Aban360].LocationPool.Zone z
	                 join [Aban360].LocationPool.Region r on z.RegionId=r.Id 
	                 join [Aban360].LocationPool.Headquarters h on r.HeadquartersId=h.Id
	                 where z.Id=@zoneId";
        }
        private string GetPaymentInfoQuery(string dbName) 
        {
            return $@"Select  
						t150.C2 as PaymentMethod,
						v.date_sabt PaymentDateJalali
					From [{dbName}].dbo.vosolab v
					Left Join [Db70].dbo.T150 t150
						On v.type_pay COLLATE SQL_Latin1_General_CP1_CI_AS = t150.C1 COLLATE SQL_Latin1_General_CP1_CI_AS
					Where 
						v.town=@zoneId AND
						v.radif=@customerNumber AND
						LTRIM(SUBSTRING(v.sh_ghabs, PATINDEX('%[^0]%', v.sh_ghabs), LEN(v.sh_ghabs)))=@billId AND
                 	    LTRIM(SUBSTRING(v.sh_pard, PATINDEX('%[^0]%', v.sh_pard), LEN(v.sh_pard)))=@payId AND
						v.date_sabt>=@billRegisterDate
					Order By v.date_sabt Desc";
        }

    }
}
