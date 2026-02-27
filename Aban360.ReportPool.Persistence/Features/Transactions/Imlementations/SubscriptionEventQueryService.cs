using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.Transactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.Transactions.Imlementations
{
    internal sealed class SubscriptionEventQueryService : AbstractBaseConnection, ISubscriptionEventQueryService
    {
        public SubscriptionEventQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>> GetEventsSummaryDtos(string billId,string fromDate)
        {
            string subscriptionDataQuery = GetSubscriptionEventsDataQuery();
            string subscriptionHeaderQuery = GetSubscriptionEventHeaderQuery();
            string waterReplacementInHeaderQuery = GetWaterReplacementDateInHeaderQuery();

            long lastRemained = 0;
            IEnumerable<WaterEventsSummaryOutputDataDto> data = await _sqlReportConnection.QueryAsync<WaterEventsSummaryOutputDataDto>(subscriptionDataQuery, new { billId = billId, fromDate = fromDate });
            if (data is not null && data.Any())
            {
                data = data.OrderBy(i => i.RegisterDate);

                for (int i = 0; i < data.Count(); i++)
                {
                    WaterEventsSummaryOutputDataDto row = data.ElementAt(i);
                    row.EventDateJalali = row.PayDateJalali == null ? row.CurrentMeterDate : row.PayDateJalali;
                    if (row.TypeCode == 7 || row.TypeCode==17 /*17: قبض ابطال شده*/)
                    {
                        row.Remained = lastRemained;
                        continue;
                    }
                    lastRemained = lastRemained + (row.DebtAmount.Value - row.CreditAmount);                    
                    row.Remained = lastRemained;
                }
            }
            WaterEventsSummaryOutputHeaderDto? header = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterEventsSummaryOutputHeaderDto>(subscriptionHeaderQuery, new { billId = billId});
            WaterReplacementInfoOutputDto? replacementInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<WaterReplacementInfoOutputDto>(waterReplacementInHeaderQuery, new { billId = billId, customerNumber = header.CustomerNumber, zoneId = header.ZoneId });
            header.WaterReplacementDate = replacementInfo is not null? replacementInfo.WaterReplacementDate:string.Empty;
            header.WaterReplacementNumber = replacementInfo is not null ? replacementInfo.WaterReplacementNumber : string.Empty;
            header.Remained = lastRemained;
            header.ReportDateJalali = DateTime.Now.ToShortPersianDateString();
            header.Title = ReportLiterals.SubscriptionEventSummary;

            var result = new ReportOutput<WaterEventsSummaryOutputHeaderDto, WaterEventsSummaryOutputDataDto>(ReportLiterals.SubscriptionEventSummary, header, data);

            return result;
        }
        public async Task<IEnumerable<WaterEventsSummaryOutputDataDto>> GetBillDto(string billId)
        {
            string query = GetSubscriptionEventsDataQuery();
            IEnumerable<WaterEventsSummaryOutputDataDto> result = await _sqlReportConnection.QueryAsync<WaterEventsSummaryOutputDataDto>(query, new { billId = billId });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;
        }
        public async Task<IEnumerable<WaterEventsSummaryOutputDataDto>> GetBillDto(int zoneId, string registerDate, string fromReadingNumber, string toReadingNumber)
        {
            string query = GetSubscriptionEventsQueryByZoneAndRegisterDay();
            IEnumerable<WaterEventsSummaryOutputDataDto> result = await _sqlReportConnection.QueryAsync<WaterEventsSummaryOutputDataDto>(query, new { zoneId, registerDate, fromReadingNumber, toReadingNumber });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;
        }
        public async Task<IEnumerable<WaterEventsSummaryOutputDataDto>> GetBillDto(int zoneId, string fromReadingNumber, string toReadingNumber)
        {
            string query = GetSubscriptionEventsQuerybyZone();
            IEnumerable<WaterEventsSummaryOutputDataDto> result = await _sqlReportConnection.QueryAsync<WaterEventsSummaryOutputDataDto>(query, new { zoneId, fromReadingNumber, toReadingNumber });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;
        }
        private string GetSubscriptionEventsDataQuery()
        {
            string query = @"
            use CustomerWarehouse
             select
                 TRIM(BillId) BillId ,
	             Id,
	             PreviousNumber PreviousMeterNumber,
	             NextNumber NextMeterNumber, 
	             PreviousDay PreviousMeterDate,
	             NextDay CurrentMeterDate,
               	 Duration,
	             RegisterDay RegisterDate,
	             SumItems DebtAmount,
	             0 CreditAmount,
	             Case When TypeCode=8 Then v.Title Else TypeId End as [Description],
	             ConsumptionAverage, 
	             Consumption,
	             NULL BankTitle,
	             NULL BankCode,
	             CommercialCount CommercialUnit,
	             DomesticCount DomesticUnit,
	             OtherCount OtherUnit,
	             EmptyCount EmptyUnit,
	             0 HouseholderNumber,
	             ContractCapacity ContractualCapacity,
	             UsageId UsageSellId,
	             UsageId2 UsageConsumptionId,
	             UsageTitle UsageSellTitle,
	             UsageTitle2 UsageConsumptionTitle,
                 NULL AS PayDateJalali,
                 TypeCode,
                 BranchTypeId,
                 BranchType BranchTypeTitle,
           	     CounterStateCode,
           	     CounterStateTitle
             from [CustomerWarehouse].dbo.Bills
             LEFT Join [Db70].dbo.CounterVaziat v On
            	CounterStateCode=v.MoshtarakinId
             where 
                (BillId)=@billId  AND
		        (@fromDate IS NULL OR
		        RegisterDay<=@fromDate)
             union
             select
	             TRIM(BillId) BillId ,
                 Id,
                 PreviousNumber PreviousMeterNumber,
                 NextNumber NextMeterNumber, 
                 PreviousDay PreviousMeterDate,
                 NextDay CurrentMeterDate,
                 0 Duration,
                 RegisterDay RegisterDate,
                 SumItems DebtAmount,
                 0 CreditAmount,
                 N'ابطال قبض' [Description],
                 0 ConsumptionAverage, 
                 Consumption,
                 NULL BankTitle,
                 NULL BankCode,
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
                 NULL AS PayDateJalali,
                 17 TypeCode,
                 0 BranchTypeId,
                 '-' BranchTypeTitle,
           	     0 CounterStateCode,
           	     '-' CounterStateTitle
             from [CustomerWarehouse].dbo.RemovedBills
             where 
                (BillId=@billId )
             union
             select
                 TRIM(BillId) BillId,
	             Id,
	             0 PreviousMeterNumber,
	             0 NextMeterNumber,
	             NULL PreviousMeterDate,
	             NULL CurrentMeterDate,
                 0 Duration,
	             RegisterDay RegisterDate,
	             0 DebtAmount, 
	             Amount CreditAmount,
	             N'پرداخت' [Description],
	             0 ConsumptionAverage,
	             0 Consumption,
	             BankName BankTitle,
	             BankCode BankCode,
	             0 CommercialUnit,
	             0 DomesticUnit,
	             0 OtherUnit,
	             0 EmptyUnit,
	             0 HouseholderNumber,
	             0 ContractCapacity,
	             0 UsageSellId,
	             0 UsageConsumptionId,
	             '' UsageSellTitle,
	             '' UsageConsumptionTitle,
                 PayDateJalali,
                 0 TypeCode,
                 0 BranchTypeId ,
                 '-' BranchTypeTitle,
           	     0 CounterStateCode,
           	     '-' CounterStateTitle
             from [CustomerWarehouse].dbo.Payments
             where 
                (BillId)=@billId  AND
		        (@fromDate IS NULL OR
		        RegisterDay<=@fromDate)";
            return query;
        }
        private string GetSubscriptionEventHeaderQuery()
        {
            return @"Select 
                    	TRIM(c.FirstName)+' '+TRIM(c.SureName) AS FullName,
                    	TRIM(c.ReadingNumber) ReadingNumber,
						c.BillId,
						TRIM(c.Address) Address,
						c.CustomerNumber,
						c.CustomerNumber AS LastCustomerNumber,
						c.BillId AS LastBillId,
						c.GuildTitle AS GuildTitle,
						'-' AS JobTitle,
                    	c.UsageTitle AS UsageTitle,
                    	c.CommercialCount+c.DomesticCount+c.OtherCount AS TotalUnit,
                    	c.ContractCapacity AS ContractualCapacity,
						0 AS HasTag,
                    	c.EmptyCount AS EmptyUnit,
                    	c.FamilyCount AS HouseholdNumber,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.MainSiphonTitle AS SiphonDiameterTitle,
                    	0 AS MeterTagCount,
                    	c.BranchType AS UsageStateTitle,
                    	c.WaterRequestDate,
                    	c.SewageRequestDate,
                    	c.WaterInstallDate AS WaterInstallationDate,
                    	c.SewageInstallDate AS SewageInstallationDate,
						'-' AS WaterReplacementDate,
						'-' AS WaterReplacementNumber,
                        c.ZoneId AS ZoneId,
						c.DeletionStateTitle DeletionStateTitle,
						c.BranchType BranchTypeTitle
                    From [CustomerWarehouse].dbo.Clients c
                    Where
						c.ToDayJalali IS NULL AND
						c.BillId=@billId ";
        }
        private string GetWaterReplacementDateInHeaderQuery()
        {
            return @"Select
                    	m.ChangeDateJalali AS WaterReplacementDate,
                    	m.MeterNumber AS WaterReplacementNumber
                    From [CustomerWarehouse].dbo.MeterChange m
                    Where
                    	m.CustomerNumber=@customerNumber AND
                    	m.ZoneId=@zoneId
                    Order By m.RegisterDateJalali Desc";
        }

        private string GetSubscriptionEventsQueryByZoneAndRegisterDay()
        {
            string query = @"
            use CustomerWarehouse
            select
	            TRIM(BillId) BillId, Id,PreviousNumber PreviousMeterNumber,NextNumber NextMeterNumber, PreviousDay PreviousMeterDate,NextDay CurrentMeterDate,RegisterDay RegisterDate,SumItems DebtAmount,0 CreditAmount,TypeId as [Description], ConsumptionAverage, NULL BankTitle
            from [CustomerWarehouse].dbo.Bills
            where 
	            ZoneId=@zoneId AND 
	            RegisterDay=@registerDate AND 
	            TRIM(ReadingNumber) BETWEEN @fromReadingNumber AND @toReadingNumber";
            return query;
        }

        public async Task<IEnumerable<BranchEventsDto>> GetBranchEventDtos(string billId)
        {
            string query = GetBranchEventsSummaryQuery();
            IEnumerable<BranchEventsDto> result = await _sqlReportConnection.QueryAsync<BranchEventsDto>(query, new { billId = billId });
            if (result.Any())
            {
                result = result.OrderBy(i => i.RegisterDate);
            }
            return result;

        }
        private string GetBranchEventsSummaryQuery()
        {
            string query =
                @"USE [CustomerWarehouse]
                SELECT 
                    N'صدور صورتحساب' [Description],
                    TrackNumber, 
                    RegisterDay RegisterDate,
                    AmountSum DebtAmount ,
                    0 CreditAmount 
                from [TerminatedRequestsV2]
                WHERE TRIM(BillId)=@billId
                UNION
                SELECT 
                    N'پرداخت'+ N'('+ BankName+' '+PaymentGateway+N')' [Description],
                    '' TrackNumber,
                    RegisterDay RegisterDate,
                    0 DebtAmount, 
                    Amount CreditAmount
                FROM [PaymentsEn]
                WHERE TRIM(BillId)=@billId";
            return query;
        }
        private string GetSubscriptionEventsQuerybyZone()
        {
            string query = @"
            use CustomerWarehouse
            select
	            TRIM(BillId) BillId, Id,PreviousNumber PreviousMeterNumber,NextNumber NextMeterNumber, PreviousDay PreviousMeterDate,NextDay CurrentMeterDate,RegisterDay RegisterDate,SumItems DebtAmount,0 OweAmount,TypeId as [Description], ConsumptionAverage, NULL BankTitle
            from [CustomerWarehouse].dbo.Bills
            where 
	            ZoneId=@zoneId AND 
	            TRIM(ReadingNumber) BETWEEN @fromReadingNumber AND @toReadingNumber";
            return query;
        }
    }
}
