using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Imlementations
{
    internal sealed class SubscriptionEventWithLastDbQueryService : AbstractBaseConnection, ISubscriptionEventWithLastDbQueryService
    {
        public SubscriptionEventWithLastDbQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> GetEventsSummaryDtos(CardexInputDto input)
        {
            string dbName = GetDbName(input.ZoneId);
            string subscriptionDataQuery = GetSubscriptionEventsDataQuery(dbName);
            string subscriptionHeaderQuery = GetSubscriptionEventHeaderQuery(dbName);
            string waterReplacementInHeaderQuery = GetWaterReplacementDateInHeaderQuery(dbName);
            string lastDebtAmount = GetLastDebtAmount(dbName);
            long lastRemained = 0;

            IEnumerable<WaterEventsSummaryOutputDataDto> data = await _sqlReportConnection.QueryAsync<WaterEventsSummaryOutputDataDto>(subscriptionDataQuery, input);

            var lastBillParams = new
            {
                zoneId = input.ZoneId,
                customerNumber = input.CustomerNumber,
                lastBillDate = data.OrderByDescending(r => r.RegisterDate).FirstOrDefault().RegisterDate//todo: validation
            };
            WaterEventsSummaryOutputDataDto latestBill = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterEventsSummaryOutputDataDto>(lastDebtAmount, lastBillParams);
            if (data is not null && data.Any())
            {
                data = data.OrderBy(i => i.RegisterDate);

                for (int i = 0; i < data.Count(); i++)
                {
                    WaterEventsSummaryOutputDataDto row = data.ElementAt(i);
                    row.EventDateJalali = row.PayDateJalali == null ? row.CurrentMeterDate : row.PayDateJalali;
                    if (row.TypeCode == 7)
                    {
                        row.Remained = lastRemained;
                        continue;
                    }
                    lastRemained = lastRemained + (row.DebtAmount.Value - row.CreditAmount);
                    row.Remained = lastRemained;
                }
            }
            WaterEventsSummaryOutputHeaderDto? header = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterEventsSummaryOutputHeaderDto>(subscriptionHeaderQuery, input);
            WaterReplacementInfoOutputDto? replacementInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterReplacementInfoOutputDto>(waterReplacementInHeaderQuery, input);
            header.WaterReplacementDate = replacementInfo is not null ? replacementInfo.WaterReplacementDate : string.Empty;
            header.WaterReplacementNumber = replacementInfo is not null ? replacementInfo.WaterReplacementNumber : string.Empty;
            header.Remained = lastRemained;
            header.ReportDateJalali = DateTime.Now.ToShortPersianDateString();
            header.Title = ReportLiterals.SubscriptionEventSummary;

            var result = new ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>(ReportLiterals.SubscriptionEventSummary, header, data);

            return result;
        }


        private string GetSubscriptionEventsDataQuery(string dbName)
        {
            return $@"Select 
						b.sh_ghabs1 BillId,
						b.id,
						b.pri_no PreviousMeterNumber,
						b.today_no NextMeterNumber,
						b.pri_date PreviousMeterDate,
						b.today_date CurrentMeterDate,
						b.modat Duration,
						b.date_bed RegisterDate,
						b.baha DebtAmount,
						0 CreditAmount,
						Case When b.cod_vas IN (0,1,2,6) Then N'قبض' Else	cv.Title End [Description],--todo
						b.rate ConsumptionAverage, 
						b.masraf Consumption,
						null BankTitle, 
						null BankCode, 
						b.tedad_mas CommercialUnit,
						b.tedad_tej DomesticUnit, 
						b.tedad_vahd OtherUnit,
						b.Khali_s EmptyUnit,
						b.ted_khane HouseholderNumber,
						b.fix_mas ContractualCapacity,
						b.cod_enshab UsageSellId,
						b.group1 UsageConsumptionId,
						t41_sell.C1 UsageSellTitle,
						t41_consumption.C1 UsageConsumptionTitle,
						'' PayDateJalali,
						b.cod_vas TypeCode
						--b.type TypeCode
					From [{dbName}].dbo.bed_bes b
					Left Join [Db70].dbo.CounterVaziat cv 
						On b.cod_vas=cv.MoshtarakinId
					Left Join [Db70].dbo.T41 t41_sell
						On b.cod_enshab=t41_sell.c0 
					Left Join [Db70].dbo.T41 t41_consumption
						On b.cod_enshab=t41_consumption.c0 
					Where 
						b.town=@zoneId AND
						b.radif=@customerNumber AND
						(b.date_bed>='1401/01/01' AND 
							(@fromDate IS NULL OR
							b.date_bed<=@fromDate)
						)
					Union
					Select 
						m.bill_id BillId,
						b.id,
						0 PreviousMeterNumber,
						0 NextMeterNumber,
						NULL PreviousMeterDate,
						NULL CurrentMeterDate,
						0 Duration,
						b.date RegisterDate,
						b.mandeh DebtAmount,
						0 CreditAmount,
						N'مانده-ابتدا' [Description],--todo
						0 ConsumptionAverage, 
						0 Consumption,
						'' BankTitle, --todo
						0 BankCode, 
						0 CommercialUnit,
						0 DomesticUnit, 
						0 OtherUnit,
						0 EmptyUnit,
						0 HouseholderNumber,
						0 ContractualCapacity,
						b.noe_ensh UsageSellId,
						b.group1 UsageConsumptionId,
						t41_sell.C1 UsageSellTitle,
						t41_consumption.C1 UsageConsumptionTitle,
						'' PayDateJalali,--todo
						0 TypeCode
					From [{dbName}].dbo.base_mand b
					Left Join [{dbName}].dbo.members m
						On b.town=m.town AND b.radif=m.radif
					Left Join [Db70].dbo.T41 t41_sell
						On b.noe_ensh=t41_sell.c0 --noe_ensh
					Left Join [Db70].dbo.T41 t41_consumption
						On b.group1=t41_consumption.c0 --group1?
					Where 
						b.town=@zoneId AND
						b.radif=@customerNumber AND
						( b.date>='1401/01/01' AND 
							(@fromDate IS NULL OR
							 b.date<=@fromDate)
						)
						Union
					Select 
						m.bill_id BillId,
						r.id,
						r.pri_no PreviousMeterNumber,
						r.today_no NextMeterNumber,
						r.pri_date PreviousMeterDate,
						r.today_date CurrentMeterDate,
						r.modat Duration,
						r.date_bed RegisterDate,
						r.baha DebtAmount,
						0 CreditAmount,--todo
						N'برگشتی' [Description],--todo
						0 ConsumptionAverage, 
						0 Consumption,
						'' BankTitle,
						0 BankCode, 
						r.tedad_tej CommercialUnit,
						r.tedad_mas DomesticUnit, 
						r.tedad_vahd OtherUnit,
						0 EmptyUnit,--todo
						r.ted_khane HouseholderNumber,
						r.rate ContractualCapacity,
						r.cod_enshab UsageSellId,
						r.group1 UsageConsumptionId,
						t41_sell.C1 UsageSellTitle,
						t41_consumption.C1 UsageConsumptionTitle,
						'' PayDateJalali,
						0 TypeCode
					From [{dbName}].dbo.REPAIR r
					Left Join [{dbName}].dbo.members m
						On r.town=m.town AND r.radif=m.radif
					Left Join [Db70].dbo.T41 t41_sell
						On r.cod_enshab=t41_sell.c0
					Left Join [Db70].dbo.T41 t41_consumption
						On r.group1=t41_consumption.c0 
					Where 
						r.town=@zoneId AND
						r.radif=@customerNumber AND
						( r.date_bed>='1401/01/01' AND 
							(@fromDate IS NULL OR
							 r.date_bed<=@fromDate)
						)
					Union
					Select 
						m.bill_id BillId,
						v.id,
						0 PreviousMeterNumber,
						0 NextMeterNumber,
						NULL PreviousMeterDate,
						NULL CurrentMeterDate,
						0 Duration,
						v.date_sabt RegisterDate,
						0 DebtAmount,
						v.pard CreditAmount,
						N'پرداخت' [Description],--todo
						0 ConsumptionAverage, 
						0 Consumption,
						v.cod_bank BankTitle, --todo
						v.cod_bank BankCode, 
						0 CommercialUnit,
						0 DomesticUnit, 
						0 OtherUnit,
						0 EmptyUnit,
						0 HouseholderNumber,
						0 ContractualCapacity,
						0 UsageSellId,
						0 UsageConsumptionId,
						'' UsageSellTitle,
						'' UsageConsumptionTitle,
						v.date_bank PayDateJalali,--todo
						0 TypeCode
					From [{dbName}].dbo.vosolab v
					Left Join [{dbName}].dbo.members m
						On v.town=m.town AND v.radif=m.radif
					Left Join [Db70].dbo.T41 t41_sell
						On v.cod_enshab=t41_sell.c0 
					Left Join [Db70].dbo.T41 t41_consumption
						On v.cod_enshab=t41_consumption.c0 
					Where 
						v.town=@zoneId AND
						v.radif=@customerNumber AND
						( v.date_sabt>='1401/01/01' AND 
							(@fromDate IS NULL OR
							 v.date_sabt<=@fromDate)
						)--todo";
        }
        private string GetSubscriptionEventHeaderQuery(string dbName)
        {
            return $@"Select 
						TRIM(m.name)+' '+TRIM(m.family) AS FullName,
						TRIM(m.eshtrak) ReadingNumber,
						m.bill_id,
						TRIM(m.address) Address,
						m.radif,
						m.radif AS LastCustomerNumber,
						m.bill_id AS LastBillId,
						'' AS GuildTitle,--todo
						'-' AS JobTitle,
						t41_sell.C1 AS UsageTitle,
						(m.tedad_mas+m.tedad_tej+m.tedad_vahd) AS TotalUnit,
						m.fix_mas AS ContractualCapacity,
						0 AS HasTag,
						m.Khali_s AS EmptyUnit,
						m.ted_khane AS HouseholdNumber,
						t5.C2 AS MeterDiameterTitle,
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
						0 AS MeterTagCount,
						t7.C1 AS UsageStateTitle,
						m.ask_ab WaterRequestDate,--todo
						m.ask_fas SewageRequestDate,--todo
						m.inst_ab AS WaterInstallationDate,
						m.inst_fas AS SewageInstallationDate,
						'-' AS WaterReplacementDate,
						'-' AS WaterReplacementNumber,
					    m.town AS ZoneId
					From [{dbName}].dbo.members m
					Left Join [Db70].dbo.T41 t41_sell
						On m.cod_enshab=t41_sell.c0 
					Left Join [Db70].dbo.T41 t41_consumption
						On m.cod_enshab=t41_consumption.c0 
					Join [Db70].dbo.T5 t5
						ON m.enshab=t5.C0
					Join [Db70].dbo.T7 t7
						ON m.noe_va=t7.C0
					Where
						m.town=@zoneId AND
						m.radif=@customerNumber ";
        }
        private string GetWaterReplacementDateInHeaderQuery(string dbName)
        {
            return @$"Select
                    	t.taviz_date AS WaterReplacementDate,
                    	t.taviz_no AS WaterReplacementNumber
                    From [{dbName}].dbo.taviz t
                    Where
                    	t.radif=@customerNumber AND
                    	t.town=@zoneId
                    Order By t.taviz_date Desc";
        }
        private string GetLastDebtAmount(string dbName)
        {
            return @$"Select top 1
						b.sh_ghabs1 BillId,
						b.id,
						0 PreviousMeterNumber,
						0 NextMeterNumber,
						'' PreviousMeterDate,
						'' CurrentMeterDate,
						0 Duration,
						'1400/12/29' RegisterDate,
						b.jam DebtAmount,
						0 CreditAmount,
						Case When b.cod_vas IN (0,1,2,6) Then N'قبض' Else	cv.Title End [Description],--todo
						0 ConsumptionAverage, 
						0 Consumption,
						null BankTitle, 
						null BankCode, 
						b.tedad_mas CommercialUnit,
						b.tedad_tej DomesticUnit, 
						b.tedad_vahd OtherUnit,
						b.Khali_s EmptyUnit,
						b.ted_khane HouseholderNumber,
						b.fix_mas ContractualCapacity,
						b.cod_enshab UsageSellId,
						b.group1 UsageConsumptionId,
						t41_sell.C1 UsageSellTitle,
						t41_consumption.C1 UsageConsumptionTitle,
						'' PayDateJalali,
						b.cod_vas TypeCode
						--b.type TypeCode
					From [{dbName}].dbo.bed_bes b
					Left Join [Db70].dbo.CounterVaziat cv 
						On b.cod_vas=cv.MoshtarakinId
					Left Join [Db70].dbo.T41 t41_sell
						On b.cod_enshab=t41_sell.c0 
					Left Join [Db70].dbo.T41 t41_consumption
						On b.cod_enshab=t41_consumption.c0 
					where 
						b.date_bed<@lastBillDate AND
						b.town=@zoneId AND
						b.radif=@customerNumber
					Order by b.date_bed DEsc";
        }
    }
}
