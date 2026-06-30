using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.InvoiceInfo.Dto;
using Aban360.ReportPool.Persistence.Features.WaterInvoice.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.WaterInvoice.Implementations
{
    internal sealed class BillQueryService : AbstractBaseConnection, IBillQueryService
    {
        public BillQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<BillItemsGetDto> Get(int id)
        {
            string query = GetItemsByBillIdQuery();
            BillItemsGetDto? data = await _sqlReportConnection.QueryFirstOrDefaultAsync<BillItemsGetDto>(query, new { id });
            if (data == null)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvoiceNotFound);
            }
            return data;
        }
        public async Task<IEnumerable<BillTransactionDetailGetDto>> GetBillDetails(string billId)
        {
            string query = GetPreviousBillsDetailsQuery();
            IEnumerable<BillTransactionDetailGetDto> result = await _sqlReportConnection.QueryAsync<BillTransactionDetailGetDto>(query, new { billId });
            return result;
        }
        public async Task<IEnumerable<BillHistoryDataOutputDto>> GetHistory(BillHistoryInputDto inputDto)
        {
            string query = GetBillHistoryQuery();
            IEnumerable<BillHistoryDataOutputDto> result = await _sqlReportConnection.QueryAsync<BillHistoryDataOutputDto>(query, inputDto);
            return result;
        }
        public async Task<IEnumerable<BillLatestListDataOutputDto>> GetLatestList(BillLatestListInputDto inputDto)
        {
            string query = GetBillLatestListQuery();
            IEnumerable<BillLatestListDataOutputDto> result = await _sqlReportConnection.QueryAsync<BillLatestListDataOutputDto>(query, inputDto, null, 180);
            return result;
        }
        public async Task<IEnumerable<BillLatestListDataOutputDto>> GetLatestListByBedBes(BillLatestListInputDto inputDto)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetBedBesLatestListQuery(dbName);
            IEnumerable<BillLatestListDataOutputDto> result = await _sqlReportConnection.QueryAsync<BillLatestListDataOutputDto>(query, inputDto, null, 180);
            return result;
        }
        public async Task<IEnumerable<BillLatestListDataOutputDto>> GetLatestForNonRead(BillLatestListInputDto inputDto, string dateJalali)
        {
            string dbName = GetDbName(inputDto.ZoneId);
            string query = GetBedBesLatestForNonReadQuery(dbName);
            IEnumerable<BillLatestListDataOutputDto> result = await _sqlReportConnection.QueryAsync<BillLatestListDataOutputDto>(query, new { inputDto.ZoneId, inputDto.FromReadingNumber, inputDto.ToReadingNumber, dateJalali }, null, 180);
            return result;
        }

        private string GetItemsByBillIdQuery()
        {
            return $@"Select 
						Id,
						BillId,
						CustomerNumber,
						t46.C0 RegionId,
						t46.C2 RegionTitle,
						ZoneId,
						ZoneTitle,
						UsageId,
						UsageTitle,
						BranchTypeId,
						BranchType BranchTypeTitle,
						Consumption	,
						ConsumptionAverage,
						Item1 AbBaha,
						Item2 FazelabBaha,
						Item3 AbonmanAb,
						Item4 AbonmanFazelab,
						Item5 Maliat,
						Item6 Tabsare2,
						Item7 Tabsare2_3,
						Item8 Jarime,
						Item9 Abresani,
						Item10  JavaniJamiat,
						Item11 FaslGarm,
						Item12 ZaribTadil,
						Item13 Tabsare3Ab,
						Item14 Tabsare3Fazelab,
						Item15 TabsareAbonmanFazelab,
						Item16 GhanonBoodje,
						Item17 JavazemKahande,
						Item18 AvarezSanati
					From CustomerWarehouse.dbo.Bills
					Left Join [Db70].dbo.T51 t51
						On ZoneId=t51.C0
					Left Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
					Where Id=@id";
        }
        private string GetPreviousBillsDetailsQuery()
        {
            return $@"Select 
						b.ZoneId,
						b.ZoneTitle,
						b.CustomerNumber,
						b.BillId,
						b.UsageId UsageSellId,
						b.UsageTitle UsageSellTitle,
						b.UsageId2 UsageConsumptionId,
						b.UsageTitle2 UsageConsumptionTitle,
						b.BranchTypeId,
						b.BranchType BranchTypeTitle,
						b.PreviousDay PreviousDateJalali,
						b.PreviousNumber,
						b.NextDay CurrentDateJalali,
						b.NextNumber,
						b.Consumption,
						b.ConsumptionAverage,
						b.SumItems,
						b.DomesticCount DomesticUnit,
						b.CommercialCount CommercialUnit,
						b.OtherCount OtherUnit,
						b.EmptyCount,
						b.CounterStateCode,
						b.CounterStateTitle,
						b.RegisterDay RegisterDateJalali
					From [CustomerWarehouse].dbo.Bills b
					Where 
						BillId=@BillId AND
						CustomerWarehouse.dbo.PersianToMiladi(RegisterDay)>=DATEADD(YEAR,-1,GETDATE())
					Order by RegisterDay ASC";
        }
        private string GetBillHistoryQuery()
        {
            return $@"Select 
						Id,
						BillId,
						CustomerNumber,
						t46.C0 RegionId,
						t46.C2 RegionTitle,
						ZoneId,
						ZoneTitle,
						UsageId,
						UsageTitle,
						BranchTypeId,
						BranchType BranchTypeTitle,
						Consumption	,
						ConsumptionAverage,
						Item1 AbBaha,
						Item2 FazelabBaha,
						Item3 AbonmanAb,
						Item4 AbonmanFazelab,
						Item5 Maliat,
						Item6 Tabsare2,
						Item7 Tabsare2_3,
						Item8 Jarime,
						Item9 Abresani,
						Item10  JavaniJamiat,
						Item11 FaslGarm,
						Item12 ZaribTadil,
						Item13 Tabsare3Ab,
						Item14 Tabsare3Fazelab,
						Item15 TabsareAbonmanFazelab,
						Item16 GhanonBoodje,
						Item17 JavazemKahande,
						Item18 AvarezSanati,
						Payable,
						SumItems
					From CustomerWarehouse.dbo.Bills
					Left Join [Db70].dbo.T51 t51
						On ZoneId=t51.C0
					Left Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
					Where 
						BillId=@BillId AND
						RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali AND
					    CounterStateCode NOT IN (4,7)
					Order by RegisterDay ASC";
        }
        private string GetBillLatestListQuery()
        {
            return $@";With Cte As(
						Select 
							b.Id,
							b.BillId,
							b.CustomerNumber,
							b.ReadingNumber,
							t46.C0 RegionId,
							t46.C2 RegionTitle,
							b.ZoneId,
							b.ZoneTitle,
							b.UsageId,
							b.UsageTitle,
							b.BranchTypeId,
							b.BranchType BranchTypeTitle,
							b.Consumption ,
							b.ConsumptionAverage,
							b.CounterStateCode,
							b.CounterStateTitle,
							b.RegisterDay RegisterDateJalali,
							Rn=ROW_NUMBER() OVER(PARTITION BY b.BillId ORDER BY b.RegisterDay DESC)
					From CustomerWarehouse.dbo.Clients c
				    Join CustomerWarehouse.dbo.Bills b
						On c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
					Left Join [Db70].dbo.T51 t51
						On b.ZoneId=t51.C0
					Left Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
					Where 
						c.ToDayJalali IS NULL AND
						c.DeletionStateId NOT IN (1,5) AND
						c.ZoneId=@ZoneId AND
						c.ReadingNumber  BETWEEN @FromReadingNumber AND @ToReadingNumber
					)
					Select Top 300 * 
					From Cte
					Where 
						Rn=1 AND
						CustomerWarehouse.dbo.PersianToMiladi(RegisterDateJalali)<DATEADD(DAY,-15,Cast(GETDATE() AS date))";
        }
        private string GetBedBesLatestListQuery(string dbName)
        {
            return $@";WITH CTE AS(
						Select 
							b.Id,
							b.sh_ghabs1 BillId,
							b.radif CustomerNumber,
							b.eshtrak ReadingNumber,
							t46.C0 RegionId,
							t46.C2 RegionTitle,
							b.town ZoneId,
							t51.C2 ZoneTitle,
							b.cod_enshab UsageId,
							t41.C1 UsageTitle,
							b.noe_va BranchTypeId,
							t7.C1 BranchTypeTitle,
							b.today_no PreviousNumber,
							b.today_date PreviousDateJalali,
							b.masraf Consumption ,
							b.rate ConsumptionAverage,
							b.cod_vas CounterStateCode,
							cv.Title CounterStateTitle,
							b.date_bed RegisterDateJalali,
							b.del IsReturned,
							Rn=ROW_NUMBER() OVER(PARTITION BY b.town,b.radif ORDER BY b.date_bed DESC)
						From [{dbName}].dbo.members m 
						Join [{dbName}].dbo.bed_bes b
							ON m.town=b.town AND m.radif=b.radif
						Left Join [Db70].dbo.T51 t51
							On b.town=t51.C0
						Left Join [Db70].dbo.T46 t46
							On t51.C1=t46.C0
						Left Join [Db70].dbo.T41 t41
							On b.cod_enshab=t41.C0
						Left Join [Db70].dbo.T7 t7
							On b.noe_va=t7.C0
						Left Join [Db70].dbo.CounterVaziat cv
							On b.cod_vas=cv.MoshtarakinId
						Where 
							m.hasf NOT IN (1,5) AND
							m.town=@ZoneId AND
							m.eshtrak BETWEEN @FromReadingNumber AND @ToReadingNumber
					)
					Select TOP 300 * 
					From CTE
					WHERE
						rn=1 AND
						CustomerWarehouse.dbo.PersianToMiladi(RegisterDateJalali)<DATEADD(DAY,-15,Cast(GETDATE() AS date))";
        }
        private string GetBedBesLatestForNonReadQuery(string dbName)
        {
            return $@";WITH CTE AS(
						Select 
							b.Id,
							b.sh_ghabs1 BillId,
							b.radif CustomerNumber,
							b.eshtrak ReadingNumber,
							t46.C0 RegionId,
							t46.C2 RegionTitle,
							b.town ZoneId,
							t51.C2 ZoneTitle,
							b.cod_enshab UsageId,
							t41.C1 UsageTitle,
							b.noe_va BranchTypeId,
							t7.C1 BranchTypeTitle,
							b.today_no PreviousNumber,
							b.today_date PreviousDateJalali,
							b.masraf Consumption ,
							b.rate ConsumptionAverage,
							b.cod_vas CounterStateCode,
							cv.Title CounterStateTitle,
							b.date_bed RegisterDateJalali,
							b.baha PreviousSumItems,
							b.del IsReturned,
							Rn=ROW_NUMBER() OVER(PARTITION BY b.town,b.radif ORDER BY b.date_bed DESC)
						From [{dbName}].dbo.members m 
						Join [{dbName}].dbo.bed_bes b
							ON m.town=b.town AND m.radif=b.radif
						Left Join [Db70].dbo.T51 t51
							On b.town=t51.C0
						Left Join [Db70].dbo.T46 t46
							On t51.C1=t46.C0
						Left Join [Db70].dbo.T41 t41
							On b.cod_enshab=t41.C0
						Left Join [Db70].dbo.T7 t7
							On b.noe_va=t7.C0
						Left Join [Db70].dbo.CounterVaziat cv
							On b.cod_vas=cv.MoshtarakinId
						Where 
							m.hasf NOT IN (1,2,5) AND
							m.town=@ZoneId AND
							m.eshtrak BETWEEN @FromReadingNumber AND @ToReadingNumber AND
							b.del=0 AND
							b.cod_vas NOT IN (4,7,8)
					)
					Select TOP 300 * 
					From CTE
					WHERE
						rn=1 AND
						RegisterDateJalali<@dateJalali";
        }
    }
}
