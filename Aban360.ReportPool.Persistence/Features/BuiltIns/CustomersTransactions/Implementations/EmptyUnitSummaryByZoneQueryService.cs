using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class EmptyUnitSummaryByZoneQueryService : AbstractBaseConnection, IEmptyUnitSummaryByZoneQueryService
    {
        public EmptyUnitSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<EmptyUnitSummaryHeaderOutputDto, EmptyUnitSummaryDataOutputDto>> GetInfo(EmptyUnitSummaryInputDto input)
        {
            string reportTitle = ReportLiterals.EmptyUnit + ReportLiterals.ByZone;
            string query = GetQuery(input.UsageSellIds.HasValue());

            IEnumerable<EmptyUnitSummaryDataOutputDto> emptyUnitData = await _sqlReportConnection.QueryAsync<EmptyUnitSummaryDataOutputDto>(query, input);
            EmptyUnitSummaryHeaderOutputDto emptyUnitHeader = new EmptyUnitSummaryHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                CustomerCount = emptyUnitData?.Sum(r => r.CustomerCount) ?? 0,
                RecordCount = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = reportTitle,

                SumDomesticUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.DomesticUnit) : 0,
                SumCommercialUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.CommercialUnit) : 0,
                SumOtherUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.OtherUnit) : 0,
                TotalUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.TotalUnit) : 0,
                SumEmptyUnit = emptyUnitData is not null && emptyUnitData.Any() ? emptyUnitData.Sum(x => x.EmptyUnit) : 0,
                Field1 = emptyUnitData?.Sum(r => r.Field1) ?? 0,
                Field2 = emptyUnitData?.Sum(r => r.Field2) ?? 0,
                Field3 = emptyUnitData?.Sum(r => r.Field3) ?? 0,
                Field4 = emptyUnitData?.Sum(r => r.Field4) ?? 0,
                Field5 = emptyUnitData?.Sum(r => r.Field5) ?? 0,
                Field6 = emptyUnitData?.Sum(r => r.Field6) ?? 0,
                MoreThan6 = emptyUnitData?.Sum(r => r.MoreThan6) ?? 0,

            };


            var result = new ReportOutput<EmptyUnitSummaryHeaderOutputDto, EmptyUnitSummaryDataOutputDto>(reportTitle, emptyUnitHeader, emptyUnitData);

            return result;
        }

        private string GetQuery(bool hasUsage)
        {
            string usageQuery = hasUsage ? "AND c.UsageId IN @usageSellIds " : string.Empty;
            return @$";WITH CTE AS
                    (
                    	SELECT 
                    	    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
                    	    *
                    	From [CustomerWarehouse].dbo.Clients c
                    	Where				
                    	    c.RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	   c.ZoneId IN @zoneIds AND
                    	    (
                    	        @fromReadingNumber IS NULL OR 
                    	        @toReadingNumber IS NULL OR
                    	        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                    	    ) AND
                    	    c.CustomerNumber<>0
                            {usageQuery}
                    )
                    Select	
                    	Max(t46.C2) AS RegionTitle,
                    	c.ZoneTitle,
						COUNT(c.ZoneTitle) CustomerCount,
						SUM(c.EmptyCount) as EmptyUnit,
						COUNT(Case When c.EmptyCount=1 Then 1 Else Null End) as Field1,
						COUNT(Case When c.EmptyCount=2 Then 1 Else Null End) as Field2,
						COUNT(Case When c.EmptyCount=3 Then 1 Else Null End) as Field3,
						COUNT(Case When c.EmptyCount=4 Then 1 Else Null End) as Field4,
						COUNT(Case When c.EmptyCount=5 Then 1 Else Null End) as Field5,
						COUNT(Case When c.EmptyCount=6 Then 1 Else Null End) as Field6,
						COUNT(Case When c.EmptyCount>6 Then 1 Else Null End) as MoreThan6,
                    	SUM(c.DomesticCount) DomesticUnit,
                    	SUM(c.CommercialCount) CommercialUnit,
                    	SUM(c.OtherCount) OtherUnit,
                        SUM(IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount))) AS TotalUnit
                     FROM CTE c
                     JOIN [Db70].dbo.T51 t51
                         On t51.C0=c.ZoneId
                     JOIN [Db70].dbo.T46 t46
                         On t51.C1=t46.C0
                     WHERE	  
						 c.RN=1 AND
                         c.DeletionStateId NOT IN(1,2) AND
                         c.EmptyCount>0
					Group By c.ZoneTitle";
        }
    }
}
