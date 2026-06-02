using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterDiscountQueryService : AbstractBaseConnection, IWaterDiscountQueryService
    {
        public WaterDiscountQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto>> GetDetail(WaterDiscountDetailInputDto input)
        {
            string title = ReportLiterals.WaterDiscountDetail;
            string query = GetDetailQuery();
            IEnumerable<WaterDiscountDetailDataOutputDto> data = await _sqlReportConnection.QueryAsync<WaterDiscountDetailDataOutputDto>(query, input);
            WaterDiscountDetailHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = data?.Count() ?? 0,
                Title = title,
                CustomerCount = data?.GroupBy(w => w.BillId)?.Select(w => w.First())?.Count() ?? 0,
                BillCount = data?.Count() ?? 0,

                AbBaha = data?.Sum(d => d.AbBaha) ?? 0,
                FazelabBaha = data?.Sum(d => d.FazelabBaha) ?? 0,
                AbonmanAb = data?.Sum(d => d.AbonmanAb) ?? 0,
                AbonmanFazelab = data?.Sum(d => d.AbonmanFazelab) ?? 0,
                Maliat = data?.Sum(d => d.Maliat) ?? 0,
                Tabsare2 = data?.Sum(d => d.Tabsare2) ?? 0,
                Tabsare2_3 = data?.Sum(d => d.Tabsare2_3) ?? 0,
                Jarime = data?.Sum(d => d.Jarime) ?? 0,
                Abresani = data?.Sum(d => d.Abresani) ?? 0,
                JavaniJamiat = data?.Sum(d => d.JavaniJamiat) ?? 0,
                FaslGarm = data?.Sum(d => d.FaslGarm) ?? 0,
                ZaribTadil = data?.Sum(d => d.ZaribTadil) ?? 0,
                Tabsare3Ab = data?.Sum(d => d.Tabsare3Ab) ?? 0,
                Tabsare3Fazelab = data?.Sum(d => d.Tabsare3Fazelab) ?? 0,
                TabsareAbonmanFazelab = data?.Sum(d => d.TabsareAbonmanFazelab) ?? 0,
                GhanonBoodje = data?.Sum(d => d.GhanonBoodje) ?? 0,
                JavazemKahande = data?.Sum(d => d.JavazemKahande) ?? 0,
                AvarezSanati = data?.Sum(d => d.AvarezSanati) ?? 0,
            };

            return new ReportOutput<WaterDiscountDetailHeaderOutputDto, WaterDiscountDetailDataOutputDto>(title, header, data);
        }
        public async Task<ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto>> GetSummary(WaterDiscountSummaryInputDto input)
        {
            var (groupFieldId, groupFieldCondition, groupFiledTitle, groupTitle) = GetGroupType(input.GroupType);
            string title = ReportLiterals.WaterDiscountSummary + $"-{groupTitle}";
            string query = GetSummaryQuery(groupFieldId, groupFieldCondition);
            IEnumerable<WaterDiscountSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<WaterDiscountSummaryDataOutputDto>(query, input);
            WaterDiscountSummaryHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = data?.Count() ?? 0,
                Title = title,
                BillCount = data?.Count() ?? 0,

                AbBaha = data?.Sum(d => d.AbBaha) ?? 0,
                FazelabBaha = data?.Sum(d => d.FazelabBaha) ?? 0,
                AbonmanAb = data?.Sum(d => d.AbonmanAb) ?? 0,
                AbonmanFazelab = data?.Sum(d => d.AbonmanFazelab) ?? 0,
                Maliat = data?.Sum(d => d.Maliat) ?? 0,
                Tabsare2 = data?.Sum(d => d.Tabsare2) ?? 0,
                Tabsare2_3 = data?.Sum(d => d.Tabsare2_3) ?? 0,
                Jarime = data?.Sum(d => d.Jarime) ?? 0,
                Abresani = data?.Sum(d => d.Abresani) ?? 0,
                JavaniJamiat = data?.Sum(d => d.JavaniJamiat) ?? 0,
                FaslGarm = data?.Sum(d => d.FaslGarm) ?? 0,
                ZaribTadil = data?.Sum(d => d.ZaribTadil) ?? 0,
                Tabsare3Ab = data?.Sum(d => d.Tabsare3Ab) ?? 0,
                Tabsare3Fazelab = data?.Sum(d => d.Tabsare3Fazelab) ?? 0,
                TabsareAbonmanFazelab = data?.Sum(d => d.TabsareAbonmanFazelab) ?? 0,
                GhanonBoodje = data?.Sum(d => d.GhanonBoodje) ?? 0,
                JavazemKahande = data?.Sum(d => d.JavazemKahande) ?? 0,
                AvarezSanati = data?.Sum(d => d.AvarezSanati) ?? 0,
            };

            return new ReportOutput<WaterDiscountSummaryHeaderOutputDto, WaterDiscountSummaryDataOutputDto>(title, header, data);
        }

        private string GetDetailQuery()
        {
            return $@"Select 
						c.ZoneId,
						c.ZoneTitle,
						t46.C0 RegionId,
						t46.C2 RegionTitle,
						c.CustomerNumber,
						c.BillId,
						c.ReadingNumber,
						c.FirstName,
						c.SureName Surname,
						c.FirstName+' '+c.SureName FullName,
						c.FatherName,
						c.PostalCode,
						c.Address,
						c.PhoneNo PhoneNumber,
						c.MobileNo MobileNumber,
						b.UsageId,
						b.UsageTitle,
						b.BranchType BranchTypeTitle,
						b.BranchTypeId,
						c.CommercialCount CommercialUnit,
						c.DomesticCount DomesticUnit,
						c.OtherCount OtherUnit,
						IIF( (b.DomesticCount+b.CommercialCount+b.OtherCount)=0,1,(b.DomesticCount+b.CommercialCount+b.OtherCount)) TotalUnit,
						IIF( (b.DomesticCount+b.CommercialCount+b.OtherCount)=0,1,(b.DomesticCount+b.CommercialCount+b.OtherCount))-b.EmptyCount  BillUnit,
						b.Consumption,
						b.ConsumptionAverage,
						b.CounterStateCode,
						b.CounterStateTitle,
						b.ItemOff1  AbBaha,
						b.ItemOff2  FazelabBaha,
						b.ItemOff3  AbonmanAb,
						b.ItemOff4  AbonmanFazelab,
						b.ItemOff5  Maliat,
						b.ItemOff6  Tabsare2,
						b.ItemOff7  Tabsare2_3,
						b.ItemOff8  Jarime,
						b.ItemOff9  Abresani,
						b.ItemOff10 JavaniJamiat,
						b.ItemOff11 FaslGarm,
						b.ItemOff12 ZaribTadil,
						b.ItemOff13 Tabsare3Ab,
						b.ItemOff14 Tabsare3Fazelab,
						b.ItemOff15 TabsareAbonmanFazelab,
						b.ItemOff16 GhanonBoodje,
						b.ItemOff17 JavazemKahande,
						b.ItemOff18 AvarezSanati
					From CustomerWarehouse.dbo.Bills b
					Join CustomerWarehouse.dbo.Clients c
						ON b.ZoneId=c.ZoneId AND b.CustomerNumber=c.CustomerNumber
					Join [Db70].dbo.T51 t51 
						ON c.ZoneId=t51.C0
					Join [Db70].dbo.T46 t46
						ON t51.C1=t46.C0 
					Where 
						c.ToDayJalali IS NULL AND
						b.ZoneId IN @ZoneIds AND
						b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali AND
						b.CounterStateCode NOT IN (4,7,8)";
        }
        private string GetSummaryQuery(string groupFieldId, string groupFieldTitle)
        {
            return $@"Select 
						{groupFieldId} ItemId,
						{groupFieldTitle} ItemTitle,
						SUM(b.CommercialCount) CommercialUnit,
						SUM(b.DomesticCount) DomesticUnit,
						SUM(b.OtherCount) OtherUnit,
						SUM(IIF( (b.DomesticCount+b.CommercialCount+b.OtherCount)=0,1,(b.DomesticCount+b.CommercialCount+b.OtherCount))) TotalUnit,
						SUM(IIF( (b.DomesticCount+b.CommercialCount+b.OtherCount)=0,1,(b.DomesticCount+b.CommercialCount+b.OtherCount))-b.EmptyCount)  BillUnit,
						SUM(b.Consumption) Consumption,
						SUM(b.ItemOff1) AbBaha,
						SUM(b.ItemOff2) FazelabBaha,
						SUM(b.ItemOff3) AbonmanAb,
						SUM(b.ItemOff4) AbonmanFazelab,
						SUM(b.ItemOff5) Maliat,
						SUM(b.ItemOff6) Tabsare2,
						SUM(b.ItemOff7) Tabsare2_3,
						SUM(b.ItemOff8) Jarime,
						SUM(b.ItemOff9) Abresani,
						SUM(b.ItemOff10) JavaniJamiat,
						SUM(b.ItemOff11) FaslGarm,
						SUM(b.ItemOff12) ZaribTadil,
						SUM(b.ItemOff13) Tabsare3Ab,
						SUM(b.ItemOff14) Tabsare3Fazelab,
						SUM(b.ItemOff15) TabsareAbonmanFazelab,
						SUM(b.ItemOff16) GhanonBoodje,
						SUM(b.ItemOff17) JavazemKahande,
						SUM(b.ItemOff18) AvarezSanati
					From CustomerWarehouse.dbo.Bills b
					Join [Db70].dbo.T51 t51 
						ON b.ZoneId=t51.C0
					Join [Db70].dbo.T46 t46
						ON t51.C1=t46.C0 
					Where 
						b.ZoneId IN @ZoneIds AND
						b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali AND
						b.CounterStateCode NOT IN (4,7,8)
					Group By {groupFieldTitle}";
        }
        private (string, string, string, string) GetGroupType(WaterDiscountGroupInputEnum input)
        {
            if (input == WaterDiscountGroupInputEnum.Region)
                return ("MAX(t46.C0)", "t46.C2", "t46.C2", ReportLiterals.ByRegion);
            if (input == WaterDiscountGroupInputEnum.Zone)
                return ("MAX(b.ZoneId)", "b.ZoneTitle", "b.ZoneTitle+'-'+MAX(t46.C2)", ReportLiterals.ByZone);
            if (input == WaterDiscountGroupInputEnum.Usage)
                return ("MAX(b.UsageId)", "b.UsageTitle", "b.UsageTitle", ReportLiterals.ByUsage);

            return ("b.ZoneId", "b.ZoneTitle", "b.ZoneTitle+'-'+MAX(t46.C2)", ReportLiterals.ByZone);
        }
    }
}