using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class SewageWaterRequestDetailQueryService : AbstractBaseConnection, ISewageWaterRequestDetailQueryService
    {
        public SewageWaterRequestDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestDetailDataOutputDto>> Get(SewageWaterRequestInputDto input)
        {
            string RequestDetailQuery;
            if (input.IsWater)
                RequestDetailQuery = GetWaterRequestDetailQuery();
            else
                RequestDetailQuery = GetSewageRequestDetailQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds
            };
            IEnumerable<SewageWaterRequestDetailDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterRequestDetailDataOutputDto>(RequestDetailQuery, @params);
            SewageWaterRequestHeaderOutputDto RequestHeader = new SewageWaterRequestHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0
            };
            var result = new ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestDetailDataOutputDto>
                (input.IsWater ? ReportLiterals.WaterRequestDetail : ReportLiterals.SewageRequestDetail,
                RequestHeader,
                RequestData);

            return result;
        }
        private string GetWaterRequestDetailQuery()
        {
            return @"Select
                    	c.CustomerNumber, 
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) AS Surname,
                    	TRIM(c.Address) AS Address,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.WaterRequestDate AS RequestDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.WaterRequestDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL";
        }
        private string GetSewageRequestDetailQuery()
        {
            return @"Select
                    	c.CustomerNumber, 
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) AS Surname,
                    	TRIM(c.Address) AS Address,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.SewageRequestDate AS RequestDate
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.SewageRequestDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL";
        }
    }
}
