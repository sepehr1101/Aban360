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
        { }
        public async Task<ReportOutput<HouseholdNumberHeaderOutputDto, HouseholdNumberDataOutputDto>> GetInfo(HouseholdNumberInputDto input, string lastYearJalali)
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

            IEnumerable<HouseholdNumberDataOutputDto> householdNumberData = await _sqlReportConnection.QueryAsync<HouseholdNumberDataOutputDto>(householdNumberQuery,@params);
            HouseholdNumberHeaderOutputDto householdNumberHeader = new HouseholdNumberHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromHouseholdDateJalali = input.FromHouseholdDateJalali,
                ToHouseholdDateJalali = input.ToHouseholdDateJalali,
                RecordCount = (householdNumberData is not null && householdNumberData.Any()) ? householdNumberData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

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
            return @"SELECT 
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
					    STUFF(c.HouseholdDateJalali,1,4,CAST(CAST(SUBSTRING(c.HouseholdDateJalali,1,4)AS int)+1 as nvarchar(4))) AS ToHouseholdDateJalali,
	            		c.FamilyCount AS HouseholdCount,
                        IIF(c.HouseholdDateJalali >@lastYearDate , 1 , 0) AS IsValid
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
	            		c.ToDayJalali IS NULL AND
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                        c.ZoneId in @zoneIds AND
	            		c.HouseholdDateJalali BETWEEN @fromHouseholdDateJalali AND @toHouseholdDateJalali";
        }
    }
}
