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
        { 
        }
        public async Task<ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberSummaryByZoneDataOutputDto>> GetInfo(HouseholdNumberInputDto input, string lastYearJalali)
        {
            string householdNumberQuery = GetHouseholdNumberQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromDate = input.FromHouseholdDateJalali,
                toDate = input.ToHouseholdDateJalali,

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
            return @";WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
							c.FamilyCount>0 AND
		                    (TRIM(c.HouseholdDateJalali)='' OR c.HouseholdDateJalali BETWEEN @fromDate AND @toDate) AND
		                    c.CustomerNumber<>0 AND
		                    c.RegisterDayJalali <= @toDate  AND
							(	@fromReadingNumber IS NULL OR
								@toReadingNumber IS NULL OR
								c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
							) AND
							c.ZoneId in @zoneIds
                    )
                    Select	
						--COUNT c.HouseholdDateJalali >@lastYearDate
						MAX(t46.C2) AS RegionTitle,
	                    c.ZoneTitle,
                        COUNT(1) CustomerCount,
						SUM(c.FamilyCount) as SumHousehold,
						SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
						SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
						SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
						SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
						Count(CASE WHEN c.FamilyCount= 1 THEN 1 ELSE Null END) AS Field1,
						Count(CASE WHEN c.FamilyCount= 2 THEN 1 ELSE Null END) AS Field2,
						Count(CASE WHEN c.FamilyCount= 3 THEN 1 ELSE Null END) AS Field3,
						Count(CASE WHEN c.FamilyCount= 4 THEN 1 ELSE Null END) AS Field4,
						Count(CASE WHEN c.FamilyCount Not In (1,2,3,4) THEN 1 ELSE Null END) AS FieldMore5						
                    FROM CTE c
					Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    WHERE	  
                        c.RN=1 AND
	                    c.DeletionStateId NOT IN(1,2)
					GROUP BY c.ZoneTitle";
        }
    }
}
