using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Contracts
{
    public interface ISubscriptionEventWithLastDbQueryService
    {
        public Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> GetEventsSummaryDtos(CardexInputDto input);
    }
    internal sealed class SubscriptionEventWithLastDbQueryService : AbstractBaseConnection, ISubscriptionEventWithLastDbQueryService
    {
        public SubscriptionEventWithLastDbQueryService(IConfiguration configuration)
            :base(configuration)
        {
        }

        public async Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> GetEventsSummaryDtos(CardexInputDto input)
        {
			string dbName = GetDbName(input.ZoneId);
            string subscriptionDataQuery = GetSubscriptionEventsDataQuery(dbName);
            //string subscriptionHeaderQuery = GetSubscriptionEventHeaderQuery(dbName);
            //string waterReplacementInHeaderQuery = GetWaterReplacementDateInHeaderQuery(dbName);

			throw new NotImplementedException();	
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
						cv.Title [Description],--todo
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
						b.type TypeCode
					From [{dbName}].dbo.bed_bes b
					Left Join [Db70].dbo.CounterVaziat cv 
						On b.type=cv.MoshtarakinId
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
    }
}
