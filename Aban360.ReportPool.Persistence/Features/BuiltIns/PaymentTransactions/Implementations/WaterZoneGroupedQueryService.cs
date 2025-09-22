using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class WaterZoneGroupedQueryService : AbstractBaseConnection, IWaterZoneGroupedQueryService
    {
        public WaterZoneGroupedQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>> GetInfo(ServiceLinkWaterItemGroupedInputDto input)
        {
            string waterZoneGroupeds = GetWaterZoneGroupedQuery();
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
            };
            IEnumerable<ServiceLinkWaterItemGroupedDataOutputDto> waterZoneGroupedData = await _sqlReportConnection.QueryAsync<ServiceLinkWaterItemGroupedDataOutputDto>(waterZoneGroupeds, @params);
            ServiceLinkWaterItemGroupedHeaderOutputDto waterZoneGroupedHeader = new ServiceLinkWaterItemGroupedHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = waterZoneGroupedData is not null && waterZoneGroupedData.Any() ? waterZoneGroupedData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                TotalAmount = waterZoneGroupedData.Sum(zone => zone.Amount),

                CustomerCount = waterZoneGroupedData is not null && waterZoneGroupedData.Any() ? waterZoneGroupedData.Count() : 0,
                SumCommercialUnit = waterZoneGroupedData?.Sum(i => i.CommercialUnit) ?? 0,
                SumDomesticUnit = waterZoneGroupedData?.Sum(i => i.DomesticUnit) ?? 0,
                SumOtherUnit = waterZoneGroupedData?.Sum(i => i.OtherUnit) ?? 0,
                TotalUnit = waterZoneGroupedData?.Sum(i => i.TotalUnit) ?? 0
            };

            var result = new ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>(ReportLiterals.WaterZoneGrouped, waterZoneGroupedHeader, waterZoneGroupedData);
            return result;
        }

        private string GetWaterZoneGroupedQuery()
        {
            return @"Select 
                    	SUM(p.Amount) AS Amount,
                    	c.ZoneTitle AS ItemTitle,
						COUNT(c.ZoneTitle) AS CustomerCount,
						SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
						SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
						SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
						SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
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
                    From [CustomerWarehouse].dbo.Payments p
                    JOIN [CustomerWarehouse].dbo.Clients c 
                    	ON p.BillId=c.BillId
					JOIN [Db70].dbo.T5 t5
						ON c.WaterDiameterId=t5.C0
                    WHERE
                        c.ToDayJalali IS NULL AND
                        (@FromDate IS NULL OR 
                        @ToDate IS NULL OR
                    	p.RegisterDay BETWEEN @FromDate and @ToDate)
                    GROUP BY c.ZoneTitle";
        }
    }
}
