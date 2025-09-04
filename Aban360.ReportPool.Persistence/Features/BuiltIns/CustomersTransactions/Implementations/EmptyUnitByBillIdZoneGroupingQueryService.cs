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
    internal sealed class EmptyUnitByBillIdZoneGroupingQueryService : AbstractBaseConnection, IEmptyUnitByBillIdZoneGroupingQueryService
    {
        public EmptyUnitByBillIdZoneGroupingQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdZoneGroupingDataOutputDto>> Get(EmptyUnitInputDto input)
        {
            string emptyUnitByBillIdZoneGroupingQuery = GetEmptyUnitByBillIdZoneGroupingQuery(input.ZoneIds.Any(), input.UsageSellIds.Any());
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                fromUnit = input.FromEmptyUnit,
                toUnit = input.ToEmptyUnit,

                usageIds = input.UsageSellIds,
                zoneIds = input.ZoneIds
            };
            IEnumerable<EmptyUnitByBillIdZoneGroupingDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<EmptyUnitByBillIdZoneGroupingDataOutputDto>(emptyUnitByBillIdZoneGroupingQuery, @params);
            EmptyUnitByBillIdSummaryHeaderOutputDto RequestHeader = new EmptyUnitByBillIdSummaryHeaderOutputDto()
            {
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = RequestData is not null && RequestData.Any() ? RequestData.Count() : 0,

                SumCommercialUnit = RequestData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = RequestData.Sum(i => i.DomesticUnit),
                SumOtherUnit = RequestData.Sum(i => i.OtherUnit),
                TotalUnit = RequestData.Sum(i => i.TotalUnit),
                EmptyUnit = RequestData.Sum(i => i.EmptyUnit),
            };
            var result = new ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdZoneGroupingDataOutputDto>
                (ReportLiterals.EmptyUnitByBillZoneGrouping,
                RequestHeader,
                RequestData);

            return result;
        }
        private string GetEmptyUnitByBillIdZoneGroupingQuery(bool hasZone, bool hasUsage)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId in @zoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND b.UsageId in @usageIds" : string.Empty;
            return @$";WITH EmptyUnitByBill AS
					(
					    SELECT
							b.ZoneTitle,
							b.CommercialCount,
							b.DomesticCount,
							b.OtherCount,
							b.WaterDiameterId,
							b.EmptyCount AS EmptyUnit,
							RN=ROW_NUMBER() over (partition by b.BillId order by b.RegisterDay Desc)
					    FROM [CustomerWarehouse].dbo.Bills b
					    WHERE
							(b.EmptyCount BETWEEN @fromUnit AND @toUnit)
							AND
							(@fromReadingNumber IS NULL OR
							@toReadingNumber IS NULL OR
							b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) 
                            {zoneQuery}
                            {usageQuery}
					)
					SELECT 
					    e.ZoneTitle AS ZoneTitle,
						COUNT(e.ZoneTitle) AS CustomerCount,
					    SUM(ISNULL(e.CommercialCount, 0) + ISNULL(e.DomesticCount, 0) + ISNULL(e.OtherCount, 0)) AS TotalUnit,
				        SUM(ISNULL(e.CommercialCount, 0)) AS CommercialUnit,
				        SUM(ISNULL(e.DomesticCount, 0)) AS DomesticUnit,
				        SUM(ISNULL(e.OtherCount, 0)) AS OtherUnit,
						SUM(ISNULL(e.EmptyUnit,0)) AS EmptyUnit,
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
			         From EmptyUnitByBill e
		   			 Join [Db70].dbo.T5 t5
						 On t5.C0=e.WaterDiameterId
				  	 Where e.RN=1
					 Group By e.ZoneTitle";
        }
    }
}