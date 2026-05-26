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
						Item18 Boodje
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
						Item18 Boodje,
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
    }
}
