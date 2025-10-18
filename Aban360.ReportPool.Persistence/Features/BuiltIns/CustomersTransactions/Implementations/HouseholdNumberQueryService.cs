using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class HouseholdNumberQueryService : AbstractBaseConnection, IHouseholdNumberQueryService
    {
        public HouseholdNumberQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }
        public async Task<ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberDataOutputDto>> GetInfo(HouseholdNumberInputDto input, string lastYearJalali)
        {
            string householdNumberQuery = GetHouseholdNumberQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                FromHouseholdDateJalali = input.FromHouseholdDateJalali,
                ToHouseholdDateJalali = input.ToHouseholdDateJalali,

                lastYearDate = lastYearJalali,
                zoneIds = input.ZoneIds
            };

            IEnumerable<HouseholdNumberDataOutputDto> householdNumberData = await _sqlReportConnection.QueryAsync<HouseholdNumberDataOutputDto>(householdNumberQuery,@params);
            HouseholdNumberHeaderOutputDto householdNumberHeader = new HouseholdNumberHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromHouseholdDateJalali = input.FromHouseholdDateJalali,
                ToHouseholdDateJalali = input.ToHouseholdDateJalali,
                RecordCount = (householdNumberData is not null && householdNumberData.Any()) ? householdNumberData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title= ReportLiterals.HouseholdNumberDetail,

                SumCommercialUnit = householdNumberData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = householdNumberData.Sum(i => i.DomesticUnit),
                SumOtherUnit = householdNumberData.Sum(i => i.OtherUnit),
                TotalUnit = householdNumberData.Sum(i => i.TotalUnit),
                CustomerCount = (householdNumberData is not null && householdNumberData.Any()) ? householdNumberData.Count() : 0,
            };

            var result = new ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberDataOutputDto>(ReportLiterals.HouseholdNumberDetail, householdNumberHeader, householdNumberData);

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
		                    (TRIM(c.HouseholdDateJalali)='' OR c.HouseholdDateJalali BETWEEN @FromHouseholdDateJalali AND @ToHouseholdDateJalali) AND
		                    c.CustomerNumber<>0 AND
		                    c.RegisterDayJalali <= @ToHouseholdDateJalali AND
							(	@fromReadingNumber IS NULL OR
								@toReadingNumber IS NULL OR
								c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
							) AND
							c.ZoneId in @zoneIds
                    )
                    Select	
                        c.RegisterDayJalali,
                        t46.C2 AS RegionTitle,
	                    c.CustomerNumber,
						c.ReadingNumber,
						TRIM(c.FirstName) AS FirstName,
						TRIM(c.SureName) As Surname,
						c.UsageTitle,
						c.WaterDiameterTitle MeterDiameterTitle,
						c.RegisterDayJalali AS EventDateJalali,
						TRIM(c.Address) AS Address,
						c.ZoneTitle,
						c.DeletionStateId,
						c.DeletionStateTitle AS UseStateTitle,
						c.DomesticCount DomesticUnit,
						c.CommercialCount CommercialUnit,
						c.OtherCount OtherUnit,
						(c.DomesticCount + c.CommercialCount + c.OtherCount) AS TotalUnit,
						TRIM(c.BillId) BillId,
						c.HouseholdDateJalali As HouseholdDateJalali,
						STUFF(TRIM(c.HouseholdDateJalali),1,4,CAST(CAST(SUBSTRING(TRIM(c.HouseholdDateJalali),1,4) AS int)+1 as nvarchar(4))) AS ToHouseholdDateJalali,
						c.FamilyCount AS HouseholdCount,
						IIF(c.HouseholdDateJalali >= @lastYearDate , 1 , 0) AS IsValid
                    FROM CTE c
                    Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    WHERE	  
                        c.RN=1 AND
	                    c.DeletionStateId NOT IN(1,2)";
        }
    }
}
