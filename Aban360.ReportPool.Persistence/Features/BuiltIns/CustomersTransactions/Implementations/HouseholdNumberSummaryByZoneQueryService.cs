using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class HouseholdNumberSummaryByZoneQueryService : AbstractBaseConnection, IHouseholdNumberSummaryByZoneQueryService
    {
        public HouseholdNumberSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { }
        public async Task<ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberSummaryByZoneDataOutputDto>> GetInfo(HouseholdNumberInputDto input, string lastYearJalali)
        {
            string householdNumberQuery = GetHouseholdNumberQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromHouseholdDateJalali = input.FromHouseholdDateJalali,
                toHouseholdDateJalali = input.ToHouseholdDateJalali,

                lastYearDate = lastYearJalali,
                zoneIds = input.ZoneIds
            };

            IEnumerable<HouseholdNumberSummaryByZoneDataOutputDto> householdNumberData = await _sqlReportConnection.QueryAsync<HouseholdNumberSummaryByZoneDataOutputDto>(householdNumberQuery, @params);
            HouseholdNumberHeaderOutputDto householdNumberHeader = new HouseholdNumberHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromHouseholdDateJalali = input.FromHouseholdDateJalali,
                ToHouseholdDateJalali = input.ToHouseholdDateJalali,
                RecordCount = householdNumberData is not null && householdNumberData.Any() ? householdNumberData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumCommercialUnit = householdNumberData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = householdNumberData.Sum(i => i.DomesticUnit),
                SumOtherUnit = householdNumberData.Sum(i => i.OtherUnit),
                TotalUnit = householdNumberData.Sum(i => i.TotalUnit),
                CustomerCount = householdNumberData.Sum(i => i.CustomerCount),
            };

            var result = new ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberSummaryByZoneDataOutputDto>(ReportLiterals.HouseholdNumberSummary + ReportLiterals.ByZone, householdNumberHeader, householdNumberData);

            return result;
        }

        private string GetHouseholdNumberQuery()
        {
            return @";WITH Households as(
                    SELECT 
                        c.UsageTitle,
                        c.ZoneTitle,
                        c.ZoneId,
						c.WaterDiameterId,
                        c.DomesticCount DomesticUnit,
	                    c.CommercialCount CommercialUnit,
	                    c.OtherCount OtherUnit,
						c.FamilyCount AS HouseholdCount,
                        IIF(c.HouseholdDateJalali >@lastYearDate , 1 , 0) AS IsValid
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
	            		c.ToDayJalali IS NULL AND
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                        c.ZoneId in @zoneIds AND
	            		c.HouseholdDateJalali BETWEEN @fromHouseholdDateJalali AND @toHouseholdDateJalali
                    )
                    Select 
                    	MAX(t46.C2) AS RegionTitle,
						h.ZoneTitle,
						AVG(h.HouseholdCount) as HouseholdAverage,
						COUNT(h.ZoneTitle) AS CustomerCount,
					    SUM(ISNULL(h.CommercialUnit, 0) + ISNULL(h.DomesticUnit, 0) + ISNULL(h.OtherUnit, 0)) AS TotalUnit,
					    SUM(ISNULL(h.CommercialUnit, 0)) AS CommercialUnit,
                        SUM(ISNULL(h.DomesticUnit, 0)) AS DomesticUnit,
                        SUM(ISNULL(h.OtherUnit, 0)) AS OtherUnit,
						SUM(CASE WHEN t5.C0 = 0 THEN 1 ELSE 0 END) AS UnSpecified,
						SUM(CASE WHEN t5.C0 = 1 THEN 1 ELSE 0 END) AS Field0_5,
						SUM(CASE WHEN t5.C0 = 2 THEN 1 ELSE 0 END) AS Field0_75,
						SUM(CASE WHEN t5.C0 = 3 THEN 1 ELSE 0 END) AS Field1,
						SUM(CASE WHEN t5.C0 = 4 THEN 1 ELSE 0 END) AS Field1_2,
						SUM(CASE WHEN t5.C0 = 5 THEN 1 ELSE 0 END) AS Field1_5,
						SUM(CASE WHEN t5.C0 = 6 THEN 1 ELSE 0 END) AS Field2,
						SUM(CASE WHEN t5.C0 = 7 THEN 1 ELSE 0 END) AS Field3,
						SUM(CASE WHEN t5.C0 = 8 THEN 1 ELSE 0 END) AS Field4,
						SUM(CASE WHEN t5.C0 = 9 THEN 1 ELSE 0 END) AS Field5,
						SUM(CASE WHEN t5.C0 In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    From Households h
                    Join [Db70].dbo.T5 t5
                    	On t5.C0=h.WaterDiameterId
					Join [Db70].dbo.T51 t51
						On t51.C0=h.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Group By h.ZoneTitle";
        }
    }
}
